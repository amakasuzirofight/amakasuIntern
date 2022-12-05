using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoundMaster
{
    public readonly struct ImpactData2nd
    {
        public readonly float damage;
        public readonly float moveDistance;
        public readonly float moveSpeed;
        public readonly Vector2 position;
        public readonly Vector2 moveVec;
        public readonly boundMoveState2nd boundMoveState2nd;
        public readonly string hitObjectName;
        public readonly int id;

        public ImpactData2nd(float damage, float moveDistance, float moveSpeed, Vector2 position, Vector2 moveVec, boundMoveState2nd boundMoveState2nd, string hitObjectName, int id)
        {
            this.damage = damage;
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
            this.position = position;
            this.moveVec = moveVec;
            this.boundMoveState2nd = boundMoveState2nd;
            this.hitObjectName = hitObjectName;
            this.id = id;
        } 
        public ImpactData2nd(float moveDistance, float moveSpeed, Vector2 position, Vector2 moveVec, boundMoveState2nd boundMoveState2nd, string hitObjectName, int id)
        {
            this.damage = 0;
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
            this.position = position;
            this.moveVec = moveVec;
            this.boundMoveState2nd = boundMoveState2nd;
            this.hitObjectName = hitObjectName;
            this.id = id;
        }
        public ImpactData2nd(string name, int id)
        {
            damage = 0;
            moveDistance = -1;
            moveSpeed = 0;
            position = Vector2.zero;
            moveVec = Vector2.zero;
            boundMoveState2nd = boundMoveState2nd.Null;
            hitObjectName = name;
            this.id = id;
        }
    }

}
