using BoundController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoundAllHitCheck_AMA
{

    //オブジェクトのあたり判定を計算　当たっているかを返す
    public static bool HitCheck(GameObject obj1, GameObject obj2)
    {
        float guriRadius = obj1.GetComponent<CircleCollider2D>().radius;
        float guraRadius = obj2.GetComponent<CircleCollider2D>().radius;
        
        Vector2 obj1Pos = obj1.transform.position;
        Vector2 obj2Pos = obj2.transform.position;

        Vector2 distanceVec = obj1Pos - obj2Pos;
        float distance = Mathf.Sqrt((Mathf.Pow(distanceVec.x, 2)) + (Mathf.Pow(distanceVec.y, 2)));

        //2点間の距離が半径の合計より小さければTrue
        return distance <= guriRadius + guraRadius;
    }
}
