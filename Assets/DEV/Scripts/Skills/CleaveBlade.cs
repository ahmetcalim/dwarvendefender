using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaveBlade : Skill
{
    [Tooltip("Prefab for the cleave area object.")]

    public GameObject CleaveArea;
    private Mob _lastEnemy;
    

    private void OnTriggerEnter(Collider other)
    {
        if (Duration <= 0) return;
        Mob e;
        if(e = other.GetComponentInParent<Mob>())
        {
            if (e == _lastEnemy) return;
            if (e.gameObject.layer == 19) return;

            GameObject c = Instantiate(CleaveArea);
            c.transform.rotation = Quaternion.identity;
            float dmg = GetComponentInParent<Weapon>().Damage * Damage;
            c.GetComponent<CleaveArea>().SetSkill(Duration, dmg, AoE, e);
            c.transform.position = transform.position;
            _lastEnemy = e;
        }
    }
}
