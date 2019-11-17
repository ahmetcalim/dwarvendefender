using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kromond : DualAxes
{
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
}
