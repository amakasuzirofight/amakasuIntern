using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoundMaster
{
    public  class WallHitData2nd
    {
        public readonly Vector2 normalVec;
        public readonly Vector2 hitPos;
        public readonly string wallName;
        public readonly int id;
        public readonly bool isHit;

        public WallHitData2nd(Vector2 normalVec, Vector2 hitPos, string wallName, int id, bool isHit = true)
        {
            this.normalVec = normalVec;
            this.hitPos = hitPos;
            this.wallName = wallName;
            this.id = id;
            this.isHit = isHit;
        }
        public WallHitData2nd(string name, int id)
        {
            normalVec = Vector2.zero;
            hitPos = Vector2.zero;
            wallName = name;
            this.id = id;
            this.isHit = false;
        }

        public override bool Equals(object obj)
        {
            return obj is WallHitData2nd nd &&
                   normalVec.Equals(nd.normalVec) &&
                   hitPos.Equals(nd.hitPos) &&
                   wallName == nd.wallName &&
                   id == nd.id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(normalVec, hitPos, wallName, id, isHit);
        }
    }
}
