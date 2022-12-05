using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using BoundMaster;
namespace Manager
{
    class PunchDataManager2nd:ISetPunchData_Manager2nd,IGetPunchData_Manager2nd  
    {
        Subject<PlayerPunchData_Bound2nd> subject = new Subject<PlayerPunchData_Bound2nd>();
        PunchDataManager2nd()
        {
            Locator<ISetPunchData_Manager2nd>.Bind(this);
            Locator<IGetPunchData_Manager2nd>.Bind(this);
        }
        IObservable<PlayerPunchData_Bound2nd> IGetPunchData_Manager2nd.GetPlayerPunchData_Bound()
        {
            return subject;
        }

        IObserver<PlayerPunchData_Bound2nd> ISetPunchData_Manager2nd.SetPlayerPunchData_Bound()
        {
            return subject;
        }
    }
}
