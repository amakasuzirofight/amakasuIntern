using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using BoundMaster;

namespace Manager
{
    class BoundDataManager2nd:IGetBoundData_Manager2nd,ISetBoundData_Manager2nd
    {
        Subject<ImpactData2nd> StartBoundSubject = new Subject<ImpactData2nd>();
        BoundDataManager2nd ()
        {
            Locator<IGetBoundData_Manager2nd>.Bind(this);
            Locator<ISetBoundData_Manager2nd>.Bind(this);
        }
        IObservable<ImpactData2nd> IGetBoundData_Manager2nd.GetStartBoundData()
        {
            //OnNextしかできないやつ
            return StartBoundSubject;
        }

       
        IObserver<ImpactData2nd> ISetBoundData_Manager2nd.SetStartBoundData()
        {  
            //Subscribeしかできないやつ
            return StartBoundSubject;
        }
    }
}
