using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BoundMaster
{
    public class BoundMove2nd : MonoBehaviour
    {
        private BoundCore2nd boundCore;
        //減速値
        private float decelerationValue = 1.0f;
        //残り移動割合
        private float distanceRatio;
        private void Awake()
        {
            boundCore = GetComponent<BoundCore2nd>();
        }
        /// <summary>
        /// 実際に動かす
        /// </summary>
        public void Move()
        {
            if (boundCore.isMove.Value == true && boundCore.isStop == false)
            {
                //Debug.Log(boundCore.moveInfo.moveDistance);
                transform.position += ((Vector3)boundCore.moveInfo.moveVec * (boundCore.moveInfo.moveSpeed * decelerationValue)) * Time.deltaTime;
                MoveSpeedDown();
            }
        }
        /// <summary>
        /// 減速処理
        /// </summary>
        private void MoveSpeedDown()
        {
            boundCore.moveInfo.moveDistance -= Time.deltaTime;
            distanceRatio = 1.0f - (boundCore.moveInfo.moveDistance / boundCore.distanceComprehensive);
            //減衰の値を出す
            decelerationValue = boundCore.decelerationValueCurve.Evaluate(distanceRatio);
            //距離が0になったら
            if (boundCore.moveInfo.moveDistance <= 0)
            {
                boundCore.moveInfo.moveDistance = 0;
                if (boundCore.isMove.Value == true) boundCore.isMove.Value = false;
                boundCore.moveInfo.boundMoveState = boundMoveState2nd.SubStay;
            }
        }

    }

}
