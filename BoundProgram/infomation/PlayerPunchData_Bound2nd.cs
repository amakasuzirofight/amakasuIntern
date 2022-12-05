using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace BoundMaster
{
    /// <summary>
    /// プレイヤーに殴られた時のデータ。バウンド用
    /// </summary>
    public struct PlayerPunchData_Bound2nd
    {
        public float moveSpeed;
        public float movePower;
        public Vector2 punchVec;
        public int punchId;
        public int id;


        public PlayerPunchData_Bound2nd(float moveSpeed, float movePower, Vector2 punchVec, int punchId,int id)
        {
            this.moveSpeed = moveSpeed;
            this.movePower = movePower;
            this.punchVec = punchVec;
            this.punchId = punchId;
            this.id = id;
        }
    }
}
