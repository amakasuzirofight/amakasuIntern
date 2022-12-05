using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using BoundMaster;
namespace Manager
{
    public class WallDataManager2nd : IGetWallHitData2nd, ISetWallHitData2nd
    {
        public Subject<WallHitData2nd> subject = new Subject<WallHitData2nd>();

        WallDataManager2nd()
        {
            Locator<IGetWallHitData2nd>.Bind(this);
            Locator<ISetWallHitData2nd>.Bind(this);
        }
        IObservable<WallHitData2nd> IGetWallHitData2nd.GetWallData()
        {
            return subject;
        }

        IObserver<WallHitData2nd> ISetWallHitData2nd.SetWallHitData()
        {
            return subject;
        }
    }
}
