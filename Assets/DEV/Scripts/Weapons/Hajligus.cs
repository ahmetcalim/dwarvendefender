using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hajligus : DualAxes
{
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }
}
