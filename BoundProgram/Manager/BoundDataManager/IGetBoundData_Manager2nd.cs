using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bound;
using BoundMaster;
using UniRx;
namespace Manager
{
    interface IGetBoundData_Manager2nd
    {
        public IObservable<ImpactData2nd> GetStartBoundData();
    }
}
