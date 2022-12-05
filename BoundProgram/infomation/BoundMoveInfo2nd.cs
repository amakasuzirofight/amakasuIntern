using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoundMaster
{ 
    public struct BoundMoveInfo2nd
    {
        public Vector2 moveVec;
        public float moveDistance;
        public float moveSpeed;
        public boundMoveState2nd boundMoveState;

        public BoundMoveInfo2nd(Vector2 moveVec, float moveDistance, float moveSpeed, boundMoveState2nd boundMoveState)
        {
            this.moveVec = moveVec;
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
            this.boundMoveState = boundMoveState;
        }
        
    }
}
