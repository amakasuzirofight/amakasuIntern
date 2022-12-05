using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using BoundMaster;
using UniRx;
using Manager;
using Enemy;

namespace BoundController
{
    public class BoundObjectController : MonoBehaviour
    {
        [Inject] BoundControllerCore controllerCore;
        [Inject] IGetDeleteRegistBoundData getDeleteRegistBoundData;
        [Inject] IGetRegistBoundData getRegistBoundData;
        // Start is called before the first frame update
        void Start()
        {
            getDeleteRegistBoundData ??= Locator<IGetDeleteRegistBoundData>.GetT();
            getDeleteRegistBoundData.DeleteRegistBoundData().Subscribe(DeleteRegistBoundObj).AddTo(this.gameObject);
            getRegistBoundData ??= Locator<IGetRegistBoundData>.GetT();
            getRegistBoundData.GetRegistBoundData().Subscribe(RegistData).AddTo(this.gameObject);
        }
        /// <summary>
        /// バウンドオブジェクトを解除
        /// </summary>
        /// <param name="registBoundData"></param>
        private void DeleteRegistBoundObj(DeleteRegistBoundData registBoundData)
        {
            //削除する場合
            int num = -1;
            for (int i = 0; i < controllerCore.boundObjectDatas.Count; i++)
            {
                if (controllerCore.boundObjectDatas[i].boundCore.id == registBoundData.id)
                {
                    num = i;
                    
                    controllerCore.boundObjectDatas.RemoveAt(num);
                }
            }
            if (num == -1)
            {

                Debug.LogError("すでにない物を消そうとしました" + registBoundData.id);
            }
            else
            {
               
            }
        }
        /// <summary>
        /// バウンドオブジェクトを登録
        /// </summary>
        /// <param name="gameObject"></param>
        //private void RegistData(GameObject gameObject)
        //{
        //    for (int i = 0; i < controllerCore.boundObjects.Count; i++)
        //    {
        //        if (gameObject == controllerCore.boundObjects[i])
        //        {
        //            Debug.LogError("すでに登録されてるよ");
        //            return;
        //        }
        //    }
        //    controllerCore.boundObjects.Add(gameObject);
        //}  
        private void RegistData(GameObject gameObject)
        {
            for (int i = 0; i < controllerCore.boundObjectDatas.Count; i++)
            {
                if (gameObject == controllerCore.boundObjectDatas[i].boundObject)
                {
                    Debug.LogError("すでに登録されてるよ");
                    return;
                }
            }
            BoundObjectData data = new BoundObjectData(gameObject, gameObject.GetComponent<BoundCore2nd>());
            controllerCore.boundObjectDatas.Add(data);
        }
    }

}
