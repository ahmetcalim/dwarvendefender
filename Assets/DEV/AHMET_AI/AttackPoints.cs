using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoints : MonoBehaviour
{
    public List<AttackPoint> attackPoints;

    AttackPoint result;
    public AttackPoint SetAttackPoint()
    {
        for (int i = 0; i < attackPoints.Count; i++)
        {
            if (!attackPoints[i].isFull)
            {
                result = attackPoints[i];
                attackPoints[i].isFull = true;
                break;
            }
        }
        return result;
    }
}
