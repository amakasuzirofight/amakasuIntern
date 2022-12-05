using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundMaster;
namespace Manager
{
    interface ISetPunchData_Manager2nd
    {
        IObserver<PlayerPunchData_Bound2nd> SetPlayerPunchData_Bound();
    }
}
