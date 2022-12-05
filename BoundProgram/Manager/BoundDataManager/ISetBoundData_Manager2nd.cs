using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundMaster;
namespace Manager
{
    interface ISetBoundData_Manager2nd
    {
        public IObserver<ImpactData2nd> SetStartBoundData();
    }
}
