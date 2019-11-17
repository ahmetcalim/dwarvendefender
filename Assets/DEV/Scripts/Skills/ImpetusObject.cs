using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpetusObject : Skill
{
    // Misc1 is force carry percentage, EffectPower is force to damage conversion percentage.
    public float ColliderRadius = .5f;
    public Vector3 ColliderOffset = Vector3.zero;

    public void SetSkill(float fMult, float p, float r, Vector3 offset)
    {
        //EffectPower = fMult; MiscParam1 = p; ColliderRadius = r; ColliderOffset = offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (!GetComponent<Hammer>()) // If not a hammer:
        {
            return;
            // Draw a cyan sphere on the enemy
            Color c = Color.cyan;
            c.a = 0.75f;
            Gizmos.color = c;

            Gizmos.DrawSphere(transform.position + ColliderOffset, ColliderRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
        if (EffectPower <= 0) return;
        Mob e = other.gameObject.GetComponentInParent<Mob>();
        if (!e) return; // Not an enemy, return.
        if (other.gameObject.GetComponentInParent<ImpetusObject>()) return; // Already has Impetus, return.

        Vector3 vel;
        if (GetComponent<Hammer>() && GetComponent<Hammer>().inHand) // Initialize velocity from hammer's velocity.
        {
            Debug.Log("Hammer-Enemy contact.");
            vel = GetComponent<Hammer>().rb.GetLastVelocity(GetComponent<Hammer>().source) * GetComponent<Hammer>().Damage;
            vel = new Vector3(vel.x, -vel.y, vel.z); // Hammer's y is inverted for some reason.
        }
        else if (GetComponent<Mob>()) // Initialize velocity from enemy's velocity.
        {
            Debug.Log("Enemy-Enemy contact.");
            vel = GetComponentInChildren<Rigidbody>().velocity * MiscParam1;
        }
        else return; // Hammer thrown, or something buggy. Return.

        Damage = EffectPower * vel.magnitude;
        e.TakeDamage(Damage); // Do the damage.

        if(e.hitPoint > 0) // If enemy is alive:
        {
            if (Damage > 1) e.Stagger(); // Can become more detailed.
        }
        if (e.hitPoint <= 0 && !e.gameObject.GetComponent<ImpetusObject>()) // If the enemy is fresh and dead:
        {
            if (GetComponent<Mob>()) // Hammer already puts velocity on the enemy by itself.
            {
                other.gameObject.GetComponentInChildren<Rigidbody>().velocity = vel;
            }
            // Set Impetus up on the enemy for more bowling fun times.
            ImpetusObject i = e.gameObject.AddComponent<ImpetusObject>() as ImpetusObject;
            i.SetSkill(EffectPower, MiscParam1, ColliderRadius, ColliderOffset);
           // SphereCollider col = e.gameObject.AddComponent<SphereCollider>() as SphereCollider;
           // col.radius = ColliderRadius;
            //col.center = ColliderOffset;
            //col.isTrigger = true;
            e.gameObject.layer = LayerMask.NameToLayer("Effect");
        }
    }
}
