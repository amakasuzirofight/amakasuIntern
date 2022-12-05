using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;
using Enemy;
using Manager;

namespace BoundMaster
{
    /// <summary>
    /// 敵と接触した際のバウンド処理
    /// </summary>
    class BoundHitReflect2nd : MonoBehaviour, IStopAction
    {
        BoundCore2nd boundCore;
        [Inject] ISetEnemyDamageData setEnemyDamageData;
        //[Inject] ISetRegistBoundData setRegistBoundData;
        //[Inject] ISetDeleteRegistBoundData setDeleteRegistBoundData;

        private List<string> oldHitBoundObjects = new List<string>();
        private List<string> nowHitBoundObjects = new List<string>();
        private List<string> sameHitBoundObjects = new List<string>();
        private void Awake()
        {
            boundCore = GetComponent<BoundCore2nd>();
            boundCore.hitCheckAct += ObjectHit;
            //setRegistBoundData = Locator<ISetRegistBoundData>.GetT();
            //setDeleteRegistBoundData = Locator<ISetDeleteRegistBoundData>.GetT();
        }
        private void Start()
        {
            setEnemyDamageData ??= Locator<ISetEnemyDamageData>.GetT();
        }
        /// <summary>
        /// 送られたデータをまとめる
        /// </summary>
        public void UnionData()
        {
            nowHitBoundObjects = GetNowObjectList();
            //今壁にめり込んでいるオブジェクトを設定
            sameHitBoundObjects = GetSameObjectList();
            Vector2 resultVec = Vector2.zero;
            float resultSpeed = 0;
            float resultDistance = 0;
            //if (boundCore.isMove.Value == true)
            //{
            for (int i = 0; i < boundCore.impactDatas.Count; i++)
            {
                //直前に判定を行ったオブジェクトは判定しない
                if (!sameHitBoundObjects.Contains(boundCore.impactDatas[i].hitObjectName))
                {
                    //Debug.Log($"あたり判定 {gameObject.name}{boundCore.moveInfo.boundMoveState}　相手{boundCore.impactDatas[i].boundMoveState2nd}");
                    switch (boundCore.impactDatas[i].boundMoveState2nd)
                    {
                        case boundMoveState2nd.MainMove:
                            //殴られていた場合
                            if (boundCore.moveInfo.boundMoveState == boundMoveState2nd.MainMove)
                            {
                                //お互い殴られていた場合は移動ベクトルを交換
                                resultVec += boundCore.impactDatas[i].moveVec;
                                //if (boundCore.DebugMode) Debug.Log(boundCore.impactDatas[i].hitObjectName + "衝突！");
                            }
                            else if (boundCore.moveInfo.boundMoveState == boundMoveState2nd.SubStay)
                            {
                                //if (boundCore.DebugMode) Debug.Log(boundCore.impactDatas[i].hitObjectName + "衝突！！");
                                //自分がとまっていたら動かす
                                boundCore.moveInfo.boundMoveState = boundMoveState2nd.SabMove;
                                if (boundCore.isMove.Value == false) boundCore.isMove.Value = true;
                                //ベクトルの算出
                                resultVec += new Vector2(
                                    transform.position.x - boundCore.impactDatas[i].position.x,
                                  transform.position.y - boundCore.impactDatas[i].position.y).normalized;
                                //速度の算出　　仕様決まってないから取り合えず加工せずに渡す
                                resultSpeed += boundCore.impactDatas[i].moveSpeed;
                                //距離も然り
                                resultDistance += boundCore.impactDatas[i].moveDistance;
                                //if (!boundCore.hitStops.Contains(this.gameObject.GetComponent<IHitStop>()))
                                //{
                                //    Debug.Log("メインに衝突された" + gameObject.name);
                                //    boundCore.hitStops.Add(this.gameObject.GetComponent<IHitStop>());
                                //}
                            }
                            //ダメージを送る
                            EnemyDamageData data = new EnemyDamageData(
                                boundCore.impactDatas[i].damage, EnemyDamageType.Collision,
                                boundCore.impactDatas[i].id);
                            setEnemyDamageData.SetEnemyDamageData().OnNext(data);
                            break;
                        case boundMoveState2nd.SabMove:
                            break;
                        case boundMoveState2nd.SubStay:
                            if (boundCore.moveInfo.boundMoveState == boundMoveState2nd.MainMove)
                            {
                                if (!boundCore.hitStops.Contains(this.gameObject.GetComponent<IHitStop>()))
                                {
                                    //Debug.Log("メインに衝突した" + gameObject.name);
                                    boundCore.hitStops.Add(this.gameObject.GetComponent<IHitStop>());
                                }
                            }
                            break;
                        case boundMoveState2nd.Null:
                            break;
                        default:
                            break;
                    }
                }

                //}
            }

            //oldHitBoundObjects = nowHitBoundObjects;
            oldHitBoundObjects.Clear();
            for (int i = 0; i < nowHitBoundObjects.Count; i++)
            {
                oldHitBoundObjects.Add(nowHitBoundObjects[i]);
            }
            boundCore.impactVec = resultVec != Vector2.zero ? resultVec.normalized : Vector2.zero;
            //衝突していて計算結果があればフラグを入れる
            if (resultVec != Vector2.zero) boundCore.isHitReflect = true;

            boundCore.moveInfo.moveSpeed = resultSpeed != 0 ? resultSpeed : boundCore.moveInfo.moveSpeed;
            boundCore.moveInfo.moveDistance = resultDistance != 0 ? resultDistance : boundCore.moveInfo.moveDistance;
            if (resultDistance != 0)
            {
                boundCore.moveInfo.moveDistance += resultDistance;
                boundCore.distanceComprehensive += resultDistance;
            }
            boundCore.impactDatas.Clear();
        }

        List<string> GetNowObjectList()
        {
            //現在の当たった情報
            List<string> result = new List<string>();
            for (int i = 0; i < boundCore.impactDatas.Count; i++)
            {
                result.Add(boundCore.impactDatas[i].hitObjectName);
            }
            return result;
        }
        /// <summary>
        /// あたり判定するオブジェクトを設定
        /// </summary>
        List<string> GetSameObjectList()
        {
            //現在の当たった情報
            List<string> result = new List<string>();
            for (int i = 0; i < nowHitBoundObjects.Count; i++)
            {
                for (int j = 0; j < oldHitBoundObjects.Count; j++)
                {
                    if (oldHitBoundObjects[j] == nowHitBoundObjects[i])
                    {
                        result.Add(oldHitBoundObjects[j]);
                    }
                }
            }
            return result;
        }
        void ObjectHit()
        {
            oldHitBoundObjects.Clear();
        }

        public void StopAction()
        {
            //DeleteRegistBoundData data = new DeleteRegistBoundData(boundCore.id);
            //setDeleteRegistBoundData.SetDeleteRegistBoundData().OnNext(data);
            boundCore.isStop = true;
        }

        public void RestartAction()
        {
            //setRegistBoundData.SetRegistBoundData().OnNext(this.gameObject);
            boundCore.isStop = false;
        }
    }
}
