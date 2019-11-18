using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointEvent : MonoBehaviour
{
    public List<GameObject> bloodPrefabs;
    public List<GameObject> smallBloodParticles;
    public List<GameObject> mediumBloodParticles;
    public List<GameObject> largetBloodParticles;
    private bool hit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (!hit)
            {
                hit = true;
                
                switch (GetComponent<Weapon>().damageLevel)
                {
                    case Weapon.DamageLevel.MIN:
                      
                        break;
                    case Weapon.DamageLevel.LOW:
                      
                        break;
                    case Weapon.DamageLevel.MED:
                        if (mediumBloodParticles.Count > 0)
                        {
                            CreateParticle(mediumBloodParticles, collision);
                        }
                      
                        break;
                    case Weapon.DamageLevel.HIGH:
                        if (mediumBloodParticles.Count > 0)
                        {
                            CreateParticle(mediumBloodParticles, collision);
                        }

                      
                        break;
                    case Weapon.DamageLevel.MAX:
                        if (largetBloodParticles.Count > 0)
                        {
                            CreateParticle(largetBloodParticles, collision);
                        }
                       
                        break;
                    default:
                        break;
                }
             
              
            }
          
        }
    }
    private void CreateParticle(List<GameObject> bloodList, Collision col)
    {
        GameObject copy = Instantiate(bloodList[Random.Range(0, bloodList.Count)], col.transform, true);
        copy.transform.position = col.GetContact(0).point;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (hit)
            {
                hit = false;
               
            }

        }
    }
}
