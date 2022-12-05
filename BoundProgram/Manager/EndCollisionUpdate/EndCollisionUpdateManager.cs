using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
namespace Manager
{
    public class EndCollisionUpdateManager:IGetEndCollision,ISetEndCollision
    {
        Subject<Unit> subject = new Subject<Unit>();

        EndCollisionUpdateManager()
        {
            Locator<IGetEndCollision>.Bind(this);
            Locator<ISetEndCollision>.Bind(this);
        }
        public IObservable<Unit> GetEndCollision()
        {
            return subject;
        }

        public IObserver<Unit> SetEndCollision()
        {
            return subject;
        }
    }
}
