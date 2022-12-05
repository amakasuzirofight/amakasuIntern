using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoundMaster
{ 
    class BoundObjectData
    {
        public readonly GameObject boundObject;
        public readonly BoundCore2nd boundCore;

        public BoundObjectData(GameObject boundObject, BoundCore2nd boundCore)
        {
            this.boundObject = boundObject;
            this.boundCore = boundCore;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundObjectData datas &&
                   EqualityComparer<GameObject>.Default.Equals(boundObject, datas.boundObject) &&
                   EqualityComparer<BoundCore2nd>.Default.Equals(boundCore, datas.boundCore);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(boundObject, boundCore);
        }
    }
}
