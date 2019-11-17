using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public List<GameObject> minDamageBloods;
    public List<GameObject> lowDamageBloods;
    public List<GameObject> midLevelBloods;
    public List<GameObject> highLevelBloods;
    public List<GameObject> maxLevelBloods;
    public void PlaceBlood(Collision collision, List<GameObject> bloodList)
    {
        GameObject bloodCopy = Instantiate(bloodList[Random.Range(0, bloodList.Count)], collision.GetContact(0).point, Quaternion.identity, collision.transform);
        bloodCopy.transform.LookAt(collision.GetContact(0).normal);
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("head"))
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 1f)
            {
                PlaceBlood(collision, maxLevelBloods);
            }
            else
            {
                switch (GetComponent<Weapon>().damageLevel)
                {
                    case Weapon.DamageLevel.MIN:
                        PlaceBlood(collision, minDamageBloods);
                        break;
                    case Weapon.DamageLevel.LOW:
                        PlaceBlood(collision, lowDamageBloods);
                        break;
                    case Weapon.DamageLevel.MED:
                        PlaceBlood(collision, midLevelBloods);
                        break;
                    case Weapon.DamageLevel.HIGH:
                        PlaceBlood(collision, highLevelBloods);
                        break;
                    case Weapon.DamageLevel.MAX:
                        PlaceBlood(collision, maxLevelBloods);
                        break;
                    default:
                        break;
                }

            }
        }
    }*/

}
