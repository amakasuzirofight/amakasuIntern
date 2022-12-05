using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Manager;
namespace BoundMaster
{
    /// <summary>
    /// プレイヤーに殴られた時の処理
    /// </summary>
    public class BoundPlayerPunchReflect2nd : MonoBehaviour
    {
        BoundCore2nd boundCore;
        [Inject] IGetEndPunch getEndPunch;
        [Inject] ISetStartBoundMove setStartPunch;
        List<int> checkIds = new List<int>();
        // Start is called before the first frame update
        void Start()
        {
            setStartPunch = Locator<ISetStartBoundMove>.GetT();
            getEndPunch = Locator<IGetEndPunch>.GetT();
            boundCore = GetComponent<BoundCore2nd>();
            boundCore.isPlayerPunch.Where(_ => _ == true).Subscribe(PunchReflect).AddTo(gameObject);
            getEndPunch.GetEndPunch().Subscribe(EndPunch).AddTo(this.gameObject);
        }
        /// <summary>
        /// パンチを食らったときのベクトルを算出
        /// </summary>
        /// <param name="_"></param>
        void PunchReflect(bool _)
        {
            if (!checkIds.Contains(boundCore.playerPunchData.punchId))
            {
                if (boundCore.isMove.Value == false) boundCore.isMove.Value = true;
                boundCore.punchReflectVec = boundCore.playerPunchData.punchVec;//ベクトルはパンチ準拠
                boundCore.isPunchReflect = true;
                boundCore.moveInfo.moveDistance = boundCore.playerPunchData.movePower + boundCore.moveInfo.moveDistance;//進む距離は今の進む距離とパンチの合計
                boundCore.moveInfo.moveSpeed = boundCore.playerPunchData.moveSpeed;//スピードもパンチ準拠
                boundCore.moveInfo.boundMoveState = boundMoveState2nd.MainMove;
                boundCore.distanceComprehensive += boundCore.playerPunchData.movePower;//上限値も延ばす;
                checkIds.Add(boundCore.playerPunchData.punchId);
                //終わりに絶対に書く
                boundCore.isPlayerPunch.Value = false;
            }
            else
            {
                boundCore.isPlayerPunch.Value = false;
            }
        }
        void EndPunch(int id)
        {
            if (checkIds.Contains(id))
            {
                checkIds.Remove(id);
            }
        }

    }
}