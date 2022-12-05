using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Manager;
namespace BoundMaster
{
    class BoundActiveRegistManager:IGetDeleteRegistBoundData,ISetDeleteRegistBoundData,ISetRegistBoundData,IGetRegistBoundData
    {
        private Subject<DeleteRegistBoundData> subject = new Subject<DeleteRegistBoundData>();
        private Subject<GameObject> subject2 = new Subject<GameObject>();
        BoundActiveRegistManager()
        {
            Locator<IGetDeleteRegistBoundData>.Bind(this);
            Locator<ISetDeleteRegistBoundData>.Bind(this);
            Locator<IGetRegistBoundData>.Bind(this);
            Locator<ISetRegistBoundData>.Bind(this);
        }

        public IObservable<DeleteRegistBoundData> DeleteRegistBoundData()
        {
            return subject;
        }

        public IObserver<DeleteRegistBoundData> SetDeleteRegistBoundData()
        {
            return subject;
        }

        IObservable<GameObject> IGetRegistBoundData.GetRegistBoundData()
        {
            return subject2;
        }

        IObserver<GameObject> ISetRegistBoundData.SetRegistBoundData()
        {
            return subject2;
        }
    }
}
