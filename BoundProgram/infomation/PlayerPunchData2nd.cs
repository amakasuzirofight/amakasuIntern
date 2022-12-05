using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoundMaster
{
    public readonly struct PlayerPunchData2nd
    {
        public readonly float punchPower;//ダメージ
        public readonly float blowPower;//吹っ飛ばし距離になる
        public readonly float blowSpeed;//吹っ飛ばし速度
        public readonly Vector2 punchVec;
        public readonly int punchId;

        public PlayerPunchData2nd(float punchPower, float blowPower, float blowSpeed, Vector2 punchVec,int punchId)
        {
            this.punchPower = punchPower;
            this.blowPower = blowPower;
            this.blowSpeed = blowSpeed;
            this.punchVec = punchVec;
            this.punchId = punchId;
        }
    }

}
