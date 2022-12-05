using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using BoundMaster;
using Enemy;
using Manager;
using Utility;
using UnityEngine.Tilemaps;
using Player;
using UniRx;

namespace BoundController
{
    /// <summary>
    /// バウンドのあたり判定を一括で行う
    /// </summary>
    class BoundUpdateController : MonoBehaviour
    {
        [Inject] ISetBoundData_Manager2nd setBoundData_Manager_Bound;
        [Inject] ISetPunchData_Manager2nd setPunchData_Manager_Bound;
        [Inject] ISetWallHitData2nd setWallHitData;
        [Inject] BoundControllerCore boundControllerCore;
        [Inject] ISetEnemyDamageData setEnemyDamageData;
        [Inject] ISetEndCollision setEndCollision;
        [SerializeField] List<GameObject> boundObject = new List<GameObject>();
        [SerializeField, Header("判定で出す例の本数")] int rayCount = 32;
        //停止するオブジェクトのリスト
        public List<int> stopObjects = new List<int>();
        private IGetPlayerAttackPower getPlayerAttackPower;
        public GameObject tilemapObj;
        Tilemap tilemap;
        private List<string> wallHits = new List<string>();
        private bool isPlayerPunchHit;
        private bool isBomberHit;
        private void Start()
        {
            setBoundData_Manager_Bound ??= Locator<ISetBoundData_Manager2nd>.GetT();
            setPunchData_Manager_Bound ??= Locator<ISetPunchData_Manager2nd>.GetT();
            setWallHitData ??= Locator<ISetWallHitData2nd>.GetT();
            setEnemyDamageData ??= Locator<ISetEnemyDamageData>.GetT();

            tilemap = tilemapObj.GetComponent<Tilemap>();
            //tilemap.WorldToCell(new Vector2(1, 2));
            for (int i = 0; i < boundObject.Count; i++)
            {
                BoundObjectData data = new BoundObjectData(boundObject[i], boundObject[i].GetComponent<BoundCore2nd>());
                boundControllerCore.boundObjectDatas.Add(data);

            }
            getPlayerAttackPower = Locator<IGetPlayerAttackPower>.GetT();
        }

        private void FixedUpdate()
        {
            BoundHitCheck();
            PlayerAttackAndWallCheck();
            UnionMoveVec();
            WallCheck();
            BoundMove();
        }
        /// <summary>
        /// 各敵同士の当たり判定をする
        /// </summary>
        void BoundHitCheck()
        {
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore1 = boundControllerCore.boundObjectDatas[i].boundCore;
                // FIXME:.tag重い
                if (boundControllerCore.boundObjectDatas[i].boundObject.tag != "Enemy")
                {
                    // FIXME:.tag重い
                    if (boundControllerCore.boundObjectDatas[i].boundObject.tag != "NotHitPunchEnemy") continue;
                }
                //ヒットストップしていたら判定しない
                if (boundCore1.isStop) continue;
                for (int j = i + 1; j < boundControllerCore.boundObjectDatas.Count; j++)
                {
                    // FIXME:.tag重い
                    if (boundControllerCore.boundObjectDatas[i].boundObject.tag != "Enemy")
                    {
                        // FIXME:.tag重い
                        if (boundControllerCore.boundObjectDatas[i].boundObject.tag != "NotHitPunchEnemy") continue;
                    }
                    // FIXME:毎フレームGetComponent
                    BoundCore2nd boundCore2 = boundControllerCore.boundObjectDatas[j].boundCore;
                    //ヒットストップしていたら判定しない
                    if (boundCore2.isStop) continue;
                    //あたり判定を行う
                    //当たっていた場合はデータを作成し送信
                    if (BoundAllHitCheck_AMA.HitCheck(
                        boundControllerCore.boundObjectDatas[i].boundObject, boundControllerCore.boundObjectDatas[j].boundObject))
                    {
                        //衝突していた場合、当たっていたデータを送る
                        //Debug.Log($"衝突を確認{boundControllerCore.boundObjects[i].name},{boundControllerCore.boundObjects[j]}");
                        //送るデータを作成
                        // FIXME:構造体大きい
                        ImpactData2nd impactData1 = CreatImpactData(boundCore1, i, boundCore2.id);
                            //new ImpactData2nd(
                            //getPlayerAttackPower.GetPlayerAttackPower(),
                            //boundCore1.moveInfo.moveDistance,
                            //boundCore1.moveInfo.moveSpeed,
                            //boundControllerCore.boundObjects[i].transform.position,
                            //boundCore1.moveInfo.moveVec,
                            //boundCore1.moveInfo.boundMoveState,
                            //boundControllerCore.boundObjects[i].name,
                            //boundCore2.id//送り先は相手
                            //);

                        ImpactData2nd impactData2 = CreatImpactData(boundCore2, j, boundCore1.id);
                            //new ImpactData2nd(
                            //getPlayerAttackPower.GetPlayerAttackPower(),
                            //boundCore2.moveInfo.moveDistance,
                            //boundCore2.moveInfo.moveSpeed,
                            //boundControllerCore.boundObjects[j].transform.position,
                            //boundCore2.moveInfo.moveVec,
                            //boundCore2.moveInfo.boundMoveState,
                            //boundControllerCore.boundObjects[j].name,
                            //boundCore1.id//送り先は相手
                            //);


                        //データを触れたオブジェクトに送信
                        setBoundData_Manager_Bound.SetStartBoundData().OnNext(impactData1);
                        setBoundData_Manager_Bound.SetStartBoundData().OnNext(impactData2);
                    }
                }
            }
            UnionHitData();
        }

        ImpactData2nd CreatImpactData(BoundCore2nd core,int num,int id)
        {
            ImpactData2nd data = new ImpactData2nd(
            getPlayerAttackPower.GetPlayerAttackPower(),
                            core.moveInfo.moveDistance,
                            core.moveInfo.moveSpeed,
                            boundControllerCore.boundObjectDatas[num].boundObject.transform.position,
                            core.moveInfo.moveVec,
                            core.moveInfo.boundMoveState,
                            boundControllerCore.boundObjectDatas[num].boundObject.name,
                            id);
            return data;
        }


        /// <summary>
        /// プレイヤーの攻撃と壁に当たっていないか判定
        /// </summary>
        void PlayerAttackAndWallCheck()
        {
            //オブジェクトごとのループ
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                //Debug.Log("boundControllerCore.boundObjects="+ boundControllerCore.boundObjects[i].name);
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore = boundControllerCore.boundObjectDatas[i].boundCore;
                //ヒットストップしていたら判定しない
                if (boundCore.isStop) continue;
                // FIXME:毎フレームGetComponent
                CircleCollider2D circleCollider = boundControllerCore.boundObjectDatas[i].boundObject.GetComponent<CircleCollider2D>();
                ContactFilter2D filter2D = new ContactFilter2D();
                filter2D.useTriggers = true;
                //とりあえず十個まで判定
                // FIXME:Arrayアロケート
                RaycastHit2D[] raycasts = new RaycastHit2D[10];
                float piece = 360 / (rayCount - 1) * Mathf.Deg2Rad;

                //パンチに当たっていないか確認
                //レイの本数のループ
                for (int j = 0; j < rayCount + 1; j++)
                {
                    //レイ角度
                    Physics2D.Raycast(boundControllerCore.boundObjectDatas[i].boundObject.transform.position,//原点から
                      new Vector2(MathF.Cos(piece * j), Mathf.Sin(piece * j)),//各角度に半径の長さ分のレイを出す
                         filter2D,
                         raycasts,
                         circleCollider.radius//半径の長さ分のレイを出す
                        );
                    for (int k = 0; k < raycasts.Length; k++)
                    {
                        if (!isPlayerPunchHit)
                        {
                            //衝突処理
                            if (raycasts[k].collider != null)
                            {
                                // FIXME:.tag重い
                                if (boundControllerCore.boundObjectDatas[i].boundObject.tag != "Enemy") continue;

                                //自分と接触しないようにする
                                if (raycasts[k].collider.gameObject == boundControllerCore.boundObjectDatas[i].boundObject) continue;
                                //パンチと接触したら
                                IGetPlayerPunchData playerAttack;
                                // FIXME:毎フレームGetComponent
                                if (raycasts[k].collider.gameObject.TryGetComponent<IGetPlayerPunchData>(out playerAttack))
                                {
                                    //パンチのデータを取得
                                    PlayerPunchData2nd playerPunchData = playerAttack.GetPlayerPunchData();
                                    //衝突していた場合データをenemyとboundに送る
                                    // FIXME:構造体大きい
                                    PlayerPunchData_Bound2nd playerPunchData_Bound = new PlayerPunchData_Bound2nd(
                                        playerPunchData.blowSpeed,
                                        playerPunchData.blowPower,
                                        playerPunchData.punchVec,
                                        playerAttack.GetPlayerPunchData().punchId,
                                        boundCore.id
                                        );
                                    //送信
                                    setPunchData_Manager_Bound.SetPlayerPunchData_Bound().OnNext(playerPunchData_Bound);
                                    isPlayerPunchHit = true;
                                    EnemyDamageData enemyDamageData = new EnemyDamageData(getPlayerAttackPower.GetPlayerAttackPower(), 0.0f,
                                        playerPunchData.punchVec, EnemyDamageType.Punch, boundControllerCore.boundObjectDatas[i].boundCore.id);
                                    setEnemyDamageData.SetEnemyDamageData().OnNext(enemyDamageData);
                                    break;
                                }
                            }
                        }
                    }
                    //壁とのあたり判定のためもう一度レイ判定を行う
                    for (int k2 = 0; k2 < raycasts.Length; k2++)
                    {
                        //入っているか確認
                        if (raycasts[k2].collider != null)
                        {
                            //自分自身は判定しない
                            if (raycasts[k2].collider.gameObject == boundControllerCore.boundObjectDatas[i].boundObject ||
                                SameChildNameCheck(boundControllerCore.boundObjectDatas[i].boundObject, raycasts[k2].collider.gameObject.name))
                            {
                                continue;
                            }
                            //地面かどうか
                            if (raycasts[k2].collider.gameObject.layer == LayerNumbers.NORMAL_GROUND_LAYER)
                            {
                                string wallName;
                                //衝突したオブジェクトがタイルマップかどうか
                                // FIXME:.name少し重い
                                if (raycasts[k2].collider.gameObject.name == tilemapObj.name)
                                {
                                    //タイルマップだったら座標を登録
                                    wallName = tilemap.WorldToCell(raycasts[k2].point).ToString() + raycasts[k2].normal;
                                }
                                else
                                {
                                    wallName = raycasts[k2].collider.name;
                                }

                                //初めての判定かどうか確認
                                if (!wallHits.Contains(wallName))
                                {
                                    //Debug.Log("壁データ送信");
                                    //データ作成
                                    WallHitData2nd sendData = new WallHitData2nd(raycasts[k2].normal,
                                    raycasts[k2].collider.ClosestPoint(raycasts[k2].collider.transform.position),
                                    wallName,
                                    // FIXME:毎フレームGetComponent
                                    boundControllerCore.boundObjectDatas[i].boundCore.id);
                                    setWallHitData.SetWallHitData().OnNext(sendData);

                                    //ダメージ作成
                                    EnemyDamageData enemyDamageData = new EnemyDamageData(getPlayerAttackPower.GetPlayerAttackPower(), 0, raycasts[k2].normal, EnemyDamageType.Wall, boundControllerCore.boundObjectDatas[i].boundCore.id);
                                    setEnemyDamageData.SetEnemyDamageData().OnNext(enemyDamageData);
                                    wallHits.Add(wallName);
                                }


                            }

                        }
                    }
                }
                //あたり判定をするオブジェクトが変わる際に空にしておく
                wallHits.Clear();
                isPlayerPunchHit = false;

            }
        }
        //敵衝突データをまとめて計算する
        private void UnionHitData()
        {
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore = boundControllerCore.boundObjectDatas[i].boundCore;
                //ヒットストップしていたら判定しない
                if (boundCore.isStop) continue;
                // FIXME:毎フレームGetComponent
                boundControllerCore.boundObjectDatas[i].boundObject.GetComponent<BoundHitReflect2nd>().UnionData();
            }
            setEndCollision.SetEndCollision().OnNext(default);
        }
        /// <summary>
        /// 壁反射ベクトルを計算する
        /// </summary>
        public void WallCheck()
        {
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore = boundControllerCore.boundObjectDatas[i].boundCore;
                //ヒットストップしていたら判定しない
                if (boundCore.isStop) continue;
                // FIXME:毎フレームGetComponent
                boundControllerCore.boundObjectDatas[i].boundObject.GetComponent<BoundWallReflect2nd>().UnionWallData();
            }
        }
        private void UnionMoveVec()
        {
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore = boundControllerCore.boundObjectDatas[i].boundCore;
                //ヒットストップしていたら判定しない
                if (boundCore.isStop) continue;
                // FIXME:毎フレームGetComponent
                boundControllerCore.boundObjectDatas[i].boundObject.GetComponent<BoundUnionMoveData>().UnionMoveData();
            }
        }
        private void BoundMove()
        {
            //IHitStop stop1 = boundControllerCore.boundObjects[0].GetComponent<IHitStop>();
            //IHitStop stop2 = boundControllerCore.boundObjects[1].GetComponent<IHitStop>();
            //    List<IHitStop> hitStops = new List<IHitStop>();
            //if (Input.GetKeyDown(KeyCode.Return))
            //{
            //    hitStops.Add(stop1);
            //    hitStops.Add(stop2);
            //    GameManager.HitStopStart(0.5f,hitStops);
            //}
            for (int i = 0; i < boundControllerCore.boundObjectDatas.Count; i++)
            {
                // FIXME:毎フレームGetComponent
                BoundCore2nd boundCore = boundControllerCore.boundObjectDatas[i].boundCore;
                //ヒットストップしていたら判定しない
                if (boundCore.isStop) continue;
                // FIXME:毎フレームGetComponent
                boundControllerCore.boundObjectDatas[i].boundObject.GetComponent<BoundMove2nd>().Move();
            }
        }
       
     
        /// <summary>
        /// 子オブジェクトと同じでないか
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        private bool SameChildNameCheck(GameObject objName, string targetName)
        {
            for (int i = 0; i < objName.transform.childCount; i++)
            {
                if (objName.transform.GetChild(i).name == targetName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
