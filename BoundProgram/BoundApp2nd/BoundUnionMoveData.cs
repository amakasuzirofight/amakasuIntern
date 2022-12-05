using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace BoundMaster
{
    public class BoundUnionMoveData : MonoBehaviour
    {
        BoundCore2nd boundCore;
        private void Awake()
        {
            boundCore = GetComponent<BoundCore2nd>();
        }
        public void UnionMoveData()
        {
            Vector2 resultVec;

            //壁に触れているか
            //if (boundCore.isWallReflect)
            //{
            //    //壁に触れていたら壁反射のベクトルに変更
            //    resultVec = boundCore.wallReflectVec;

            //    if (boundCore.DebugMode) Debug.Log("壁反射" + resultVec);
            //}
            //else
            //{

            //敵と接触していなかった場合
            if (boundCore.isHitReflect)
            {
                //敵のみに触れていた場合
                if (boundCore.isPunchReflect)
                {
                    //敵とパンチが触れていたらパンチを優先する
                    resultVec = boundCore.punchReflectVec;
                    //if (boundCore.DebugMode) Debug.Log("パンチ反射" + resultVec);

                }
                else
                {
                    //敵接触ベクトルになる
                    resultVec = boundCore.impactVec;
                    //if (boundCore.DebugMode) Debug.Log("敵との反射" + resultVec);
                }
            }
            //敵と接触していた場合
            else
            {
                if (boundCore.isPunchReflect)
                {
                    //パンチのみに当たっていた場合
                    //殴られたベクトルになる
                    resultVec = boundCore.punchReflectVec;
                    //if (boundCore.DebugMode) Debug.Log("パンチ反射" + resultVec);
                }
                else
                {
                    //何とも当たっていなかった場合
                    resultVec = boundCore.moveInfo.moveVec;
                    //if (boundCore.DebugMode) Debug.Log("パンチと敵に当たっていない" + resultVec);
                }
            }
            //}
            //Debug.Log($"壁判定前ベクトル{resultVec} " + gameObject.name);
            //動くデータ入力
            boundCore.resultPunchColissionVec = resultVec;

            //参照データ初期化
            boundCore.punchReflectVec = Vector2.zero;
            boundCore.wallReflectVec = Vector2.zero;
            boundCore.impactVec = Vector2.zero;
            boundCore.isPunchReflect = false;
            boundCore.isWallReflect = false;
            boundCore.isHitReflect = false;

            //当たっていたオブジェクトのヒットストップを開始
           　//if (boundCore.hitStops.Count != 0) Debug.Log($"ヒットストップcount={boundCore.hitStops.Count}");
            //GameManager.HitStopStart(0.3f, boundCore.hitStops);
            //boundCore.hitStops.Clear();
        }

    }

}
