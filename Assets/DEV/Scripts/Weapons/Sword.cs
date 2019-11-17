using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public override void CollisionEnter(Collision collision)
    {
        base.CollisionEnter(collision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }

    private void Start()
    {
        Skills[0] = FindObjectOfType<JudgementTrigger>();
        Skills[2] = GetComponent<Drilled>();

        StartCoroutine(SeekUpgrades(0.1f, 2));

    }
}
