using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Manager;
using Enemy;
namespace BoundMaster
{

    public class BoundCore2nd : MonoBehaviour, ISetImpactData2nd
    {
        [Inject] IGetBoundData_Manager2nd getBoundData;
        [Inject] IGetPunchData_Manager2nd getPunchData;
        [Inject] IGetWallHitData2nd getWallHitData;
        [Header("減速率")] public AnimationCurve decelerationValueCurve;
        public int id;

        /// これがバウンドの動くときに参照する値
        public BoundMoveInfo2nd moveInfo = new BoundMoveInfo2nd(new Vector2(0, 0), 0, 2, boundMoveState2nd.SubStay);

        // 敵と接触したときに入るデータ
        [HideInInspector] public List<ImpactData2nd> impactDatas = new List<ImpactData2nd>();
        //敵と接触したときのベクトル
        [HideInInspector] public Vector2 impactVec;
        public bool DebugMode;
        //殴られたときに入るデータ
        public PlayerPunchData_Bound2nd playerPunchData = new PlayerPunchData_Bound2nd(0.0f, 0.0f, Vector2.zero, -1, -1);
        //殴られた時のベクトル
        [HideInInspector] public Vector2 punchReflectVec;
        //他の壁と接触したときのデータ
        [HideInInspector] public List<WallHitData2nd> wallHitDatas = new List<WallHitData2nd>();
        //壁に反射するベクトル
        [HideInInspector] public Vector2 wallReflectVec;
        //移動距離の元
        [HideInInspector] public float distanceComprehensive;
        //各衝突フラグ
        [HideInInspector] public bool isWallReflect = false;
        [HideInInspector] public bool isPunchReflect = false;
        [HideInInspector] public bool isHitReflect = false;

        [HideInInspector] public Vector2 resultPunchColissionVec;
        [HideInInspector] public BoolReactiveProperty isPlayerPunch = new BoolReactiveProperty();
        public Action hitWallAct;
        public Action hitCheckAct;
        public List<IHitStop> hitStops = new List<IHitStop>();
        //動いているかフラグ
        [HideInInspector] public BoolReactiveProperty isMove = new BoolReactiveProperty();
        public bool isStop = false;
        private void Start()
        {
            getBoundData ??= Locator<IGetBoundData_Manager2nd>.GetT();
            getPunchData ??= Locator<IGetPunchData_Manager2nd>.GetT();
            getWallHitData ??= Locator<IGetWallHitData2nd>.GetT();
            //これは後で消す必要あり
            id = GetComponent<IGetBoundId>().GetBoundId();

            getBoundData.GetStartBoundData().Where(_ => _.id == id).Subscribe(SetStartBoundData).AddTo(this);
            getPunchData.GetPlayerPunchData_Bound().Where(_ => _.id == id).Subscribe(SetPlayerPunchData).AddTo(this);
            getWallHitData.GetWallData().Where(_ => _.id == id).Subscribe(SetWallHitData).AddTo(this);
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Reflash()
        {
            impactDatas.Clear();
            wallHitDatas.Clear();
            playerPunchData = new PlayerPunchData_Bound2nd(0.0f, 0.0f, Vector2.zero, -1, -1);
            moveInfo.moveDistance = 0;
            if (isMove.Value == true) isMove.Value = false;
            moveInfo.boundMoveState = boundMoveState2nd.SubStay;
            resultPunchColissionVec = Vector2.zero;
        }
        void SetStartBoundData(ImpactData2nd _impactData)
        {
            //Debug.Log("衝突データきた"+gameObject.name);
            impactDatas.Add(_impactData);
        }
        void SetPlayerPunchData(PlayerPunchData_Bound2nd _playerPunchData)
        {
            playerPunchData = _playerPunchData;
            isPlayerPunch.Value = true;
        }
        void SetWallHitData(WallHitData2nd _wallHitData)
        {
            wallHitDatas.Add(_wallHitData);
        }

        void ISetImpactData2nd.SetImpactData2nd(ImpactData2nd enemyDamageData)
        {
            impactDatas.Add(enemyDamageData);
        }
    }

}