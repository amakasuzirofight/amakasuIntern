using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using BoundMaster;
namespace Manager
{
    interface IGetWallHitData2nd
    {
        public IObservable<WallHitData2nd> GetWallData();
    }
}
