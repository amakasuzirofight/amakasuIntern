using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Manager;
using UniRx;
using Zenject;
namespace BoundMaster
{
    /// <summary>
    /// 壁の反射を行う
    /// </summary>
    class BoundWallReflect2nd : MonoBehaviour
    {
        BoundCore2nd boundCore;
        private List<string> oldhitWallNames = new List<string>();
        private List<string> nowhitWallNames = new List<string>();
        private List<string> samehitWallNames = new List<string>();
        int count = 0;
        private void Start()
        {
            boundCore = GetComponent<BoundCore2nd>();
        }

        public void UnionWallData()
        {
            nowhitWallNames = GetNowWallList();

            samehitWallNames = SameObjects();

            Vector2 resultVec =boundCore.resultPunchColissionVec;
            Vector2 normalVec = Vector2.zero;
            if (boundCore.wallHitDatas.Count != 0)
            {
                for (int i = 0; i < boundCore.wallHitDatas.Count; i++)
                {
                    //もし直前にあたり判定をした壁と接触したら判定しない
                    if (!samehitWallNames.Contains(boundCore.wallHitDatas[i].wallName))
                    {
                        //正規の当たり方をしているか調べる
                        Vector2 judgeVec = boundCore.resultPunchColissionVec;
                        if (IsRegularVec(judgeVec, boundCore.wallHitDatas[i].normalVec))
                        {
                            //反射ベクトルを出す
                            //当たった場所を足す
                            count++;
                            normalVec += boundCore.wallHitDatas[i].normalVec;
                        }
                        ///*   if (boundCore.DebugMode)*/ Debug.Log($"当たった壁の名前{boundCore.wallHitDatas[i].wallName}");
                    }
                    else
                    {
                        //if (boundCore.isMove.Value == true)
                        //{
                        //    resultVec = boundCore.moveInfo.moveVec;
                        //}
                        //else
                        //{
                        //    resultVec = boundCore.resultPunchColissionVec;
                        //}
                        //Debug.Log("すでにあたり判定をしていた" + resultVec);
                    }
                }
                oldhitWallNames.Clear();
                if (boundCore.isMove.Value == true)
                {
                    for (int i = 0; i < nowhitWallNames.Count; i++)
                    {
                        oldhitWallNames.Add(nowhitWallNames[i]);
                    }
                }

                if (count > 0)
                {
                    //平均を出す
                    normalVec /= count;
                    //Debug.Log("平均normal=" + normalVec);
                    //Vector2 judgeVec = Vector2.zero; ;
                    //if (boundCore.isMove.Value is true)
                    //{
                    //    judgeVec = boundCore.moveInfo.moveVec;
                    //}
                    //else
                    //{
                    //    judgeVec = boundCore.resultPunchColissionVec;
                    //}
                    //Debug.Log($"壁反射計算 vec={boundCore.resultPunchColissionVec} n={normalVec.normalized}");
                    resultVec = Vector2.Reflect(boundCore.resultPunchColissionVec, normalVec.normalized);
                }


            }
            Complate(resultVec);
        }
        /// <summary>
        /// 処理完了時に呼ぶ
        /// </summary>
        void Complate(Vector2 reflectVec)
        {
            //boundCore.wallReflectVec = reflectVec;
            boundCore.moveInfo.moveVec = reflectVec;

            //if (boundCore.isMove.Value) Debug.Log("最終移動ベクトル=" + reflectVec);
            //壁に衝突していて計算結果が出ていたら壁反射フラグを入れる

            //計算が終わったので初期化
            boundCore.resultPunchColissionVec = Vector2.zero;
            boundCore.isWallReflect = true;
            count = 0;
            oldhitWallNames = new List<string>();
            if (boundCore.isMove.Value == true)
            {
                for (int i = 0; i < nowhitWallNames.Count; i++)
                {
                    oldhitWallNames.Add(nowhitWallNames[i]);
                }
            }

            boundCore.wallHitDatas.Clear();
        }
        List<string> GetNowWallList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < boundCore.wallHitDatas.Count; i++)
            {
                result.Add(boundCore.wallHitDatas[i].wallName);

            }
            return result;
        }
        List<string> SameObjects()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < oldhitWallNames.Count; i++)
            {
                for (int j = 0; j < nowhitWallNames.Count; j++)
                {
                    if (oldhitWallNames[i].Equals(nowhitWallNames[j]))
                    {
                        result.Add(oldhitWallNames[i]);
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// 正規のベクトルなのかを判断する
        /// </summary>
        /// <returns></returns>
        private bool IsRegularVec(Vector2 moveVec, Vector2 normalVec)
        {
            bool result = false;
            //XかYどっちに値が入っているか調べる
            //if (boundCore.DebugMode) Debug.Log("normalVec" + normalVec);
            //if (boundCore.DebugMode) Debug.Log("moveVec" + moveVec);
            Vector2 compareMoveVec = new Vector2(MySign(moveVec.x), MySign(moveVec.y));
            //Debug.Log(moveVec);

            if (Mathf.Abs(normalVec.x) <= 0.001f)
            {
                //ｙに値が入っているとき
                if (MySign(normalVec.y) == compareMoveVec.y)
                {
                    //Debug.Log("ｘは０でｙは一緒");
                    //Debug.Log($"防御 n={MySign(normalVec.y)}, m={compareMoveVec.y}");
                    result = false;
                }
                else
                {
                    //Debug.Log("ベクトル検査通過");

                    result = true;
                }
            }
            else if (Mathf.Abs(normalVec.y) <= 0.001f)
            {
                //Xに値が入っているとき
                if (MySign(normalVec.x) == (compareMoveVec.x))
                {
                    //Debug.Log("防御");
                    result = false;
                }
                else
                {
                    //Debug.Log("ベクトル検査通過");
                    result = true;
                }
            }

            //Debug.LogError("例外的な値を検知しました");
            return result;
        }
        /// <summary>
        /// 数値を1,0,-1に分ける
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int MySign(float value)
        {

            if (value > 0.0f)
            {
                return 1;
            }
            else if (value < 0.0f)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        struct WallDataLocal
        {
            public string name;
            public Vector2 normalVec;

            public WallDataLocal(string name, Vector2 normalVec)
            {
                this.name = name;
                this.normalVec = normalVec;
            }

            public override bool Equals(object obj)
            {
                return obj is WallDataLocal data &&
                       name == data.name &&
                       normalVec.Equals(data.normalVec);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(name, normalVec);
            }
        }
    }
}
