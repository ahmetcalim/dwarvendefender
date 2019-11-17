using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binosh : Hammer
{
    public override void ColEnter(Collision col)
    {
        base.ColEnter(col);
    }
    public override void ColExit(Collision col)
    {
        base.ColExit(col);
    }
    private void OnCollisionExit(Collision collision)
    {
        ColExit(collision);
    }
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ColEnter(collision);
        CollisionEnter(collision);
    }
   
}
