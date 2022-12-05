using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
namespace Manager
{
    public interface ISetEndCollision
    {
       public IObserver<Unit> SetEndCollision();
    }
}
