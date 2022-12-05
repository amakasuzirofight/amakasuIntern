using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoundMaster
{
    interface IGetDeleteRegistBoundData
    {
        public IObservable<DeleteRegistBoundData> DeleteRegistBoundData();
    }
}
