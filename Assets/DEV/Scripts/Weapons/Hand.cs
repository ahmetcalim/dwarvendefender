using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Hammer
{
    public override void ColEnter(Collision col)
    {
        base.ColEnter(col);
    }
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
        ColEnter(collision);
    }
}
