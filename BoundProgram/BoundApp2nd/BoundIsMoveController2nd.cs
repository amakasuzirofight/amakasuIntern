using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager;
using UniRx;
using Zenject;
using UnityEngine;

namespace BoundMaster
{
    class BoundIsMoveController2nd : MonoBehaviour
    {
        [Inject] ISetStartBoundMove setStartPunch;
        [Inject] ISetEndBoundMove setEndBoundMove;

        BoundCore2nd boundCore;
        [SerializeField] bool debugMode;
        private void Start()
        {
            setEndBoundMove ??= Locator<ISetEndBoundMove>.GetT();
            setStartPunch ??= Locator<ISetStartBoundMove>.GetT();
            boundCore = GetComponent<BoundCore2nd>();
            boundCore.isMove.Subscribe(PunchDataCreate).AddTo(gameObject);
        }
        void PunchDataCreate(bool isMove)
        {
            if (isMove)
            {
                //Debug.Log("バウンド開始処理");
                setStartPunch.SetBoundMove().OnNext(boundCore.id);
            }
            else
            {
                //if (debugMode) AmaDebug.LogBlue("バウンド停止処理" + gameObject.name);
                boundCore.distanceComprehensive = 0;
                setEndBoundMove.SetEndBoundMove().OnNext(boundCore.id);
            }
        }
    }
}
