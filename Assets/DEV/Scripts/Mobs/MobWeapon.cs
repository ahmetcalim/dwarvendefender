using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobWeapon : MonoBehaviour
{
    public float Damage;
    private bool damaged;
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {

        if (!damaged)
        {
            if (GetComponentInParent<MobParent>())
            {
                if (!animator)
                {
                    animator = GetComponentInParent<MobParent>().mob.animator;

                }
                if (animator.GetInteger("Attack") != 0)
                {
                    if (other.GetComponentInChildren<PlayerHealthTracker>())
                    {
                        damaged = true;
                        other.GetComponentInChildren<PlayerHealthTracker>().TakeDamage(Damage);
                    }
                    if (other.GetComponent<Spire>())
                    {
                        damaged = true;
                        other.GetComponent<Spire>().TakeDamage(Damage);
                    }
                    else if (other.GetComponentInParent<Spire>())
                    {
                        damaged = true;
                        other.GetComponentInParent<Spire>().TakeDamage(Damage);
                    }
                }
            }
           
       
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInChildren<PlayerHealthTracker>())
        {
            damaged = false;
        }
        if (other.GetComponent<Spire>())
        {
            damaged = false;
        }
        else if (other.GetComponentInParent<Spire>())
        {
            damaged = false;
        }
    }
}
