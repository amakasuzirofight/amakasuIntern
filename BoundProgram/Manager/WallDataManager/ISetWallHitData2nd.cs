using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundMaster;
namespace Manager
{
    interface ISetWallHitData2nd
    {
       public IObserver<WallHitData2nd> SetWallHitData();
    }
}
