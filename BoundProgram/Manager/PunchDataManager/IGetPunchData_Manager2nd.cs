using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundMaster;
namespace Manager
{
    interface IGetPunchData_Manager2nd
    {
        IObservable<PlayerPunchData_Bound2nd> GetPlayerPunchData_Bound();
    }
}
