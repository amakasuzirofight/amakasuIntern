using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoundMaster
{
    public readonly struct DeleteRegistBoundData
    {
        public readonly int id;

        public DeleteRegistBoundData( int id)
        {
            this.id = id;
        }

        public override bool Equals(object obj)
        {
            return obj is DeleteRegistBoundData data &&
                   id == data.id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }
    }
}
