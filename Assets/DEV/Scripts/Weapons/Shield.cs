﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Hammer
{
    private void Awake()
    {
        isHammer = false;
    }
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
    public override void ColEnter(Collision col)
    {
        base.ColEnter(col);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
        ColEnter(collision);
    }
}
