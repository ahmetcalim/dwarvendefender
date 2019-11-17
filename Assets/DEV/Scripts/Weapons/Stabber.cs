using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabber : MonoBehaviour
{
    public Weapon weapon;
    private bool stabbed;
    public WeaponThrowingManager throwingManager;
    private Rigidbody weaponRB;
    public WeaponBaseBehaviour baseBehaviour;
    public Animator handAnimator;
    public bool comingBack;
    private void OnTriggerEnter(Collider other)
    {
        weaponRB = throwingManager.weaponRB;
        if (other.CompareTag("enemy"))
        {
                if (comingBack)
                {
                    return;
                }
                if (other.GetComponent<Rigidbody>() && weaponRB.GetComponent<FixedJoint>())
                {
                    weaponRB.transform.SetParent(null);
                    weaponRB.constraints = RigidbodyConstraints.None;
                    weaponRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    baseBehaviour.GetComponent<BoxCollider>().enabled = false;
                    throwingManager.isThrowing = true;
                    handAnimator.SetTrigger("idle");
                    weaponRB.gameObject.GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
                    weaponRB.GetComponent<FixedJoint>().enablePreprocessing = false;
                    weaponRB.GetComponent<FixedJoint>().breakForce = 2000f;
                    weaponRB.GetComponent<FixedJoint>().breakTorque = 2000f;
            }
                else
                {
                    if (weaponRB.GetComponent<Hammer>())
                    {
                        weaponRB.GetComponent<Hammer>().inHand = false;
                    }

                    weaponRB.transform.SetParent(null);
                    weaponRB.constraints = RigidbodyConstraints.None;
                    weaponRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    baseBehaviour.GetComponent<BoxCollider>().enabled = false;

                    throwingManager.isThrowing = true;
                    handAnimator.SetTrigger("idle");
                    weaponRB.gameObject.AddComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
                    weaponRB.GetComponent<FixedJoint>().enablePreprocessing = false;
                    weaponRB.GetComponent<FixedJoint>().breakForce = 2000f;
                    weaponRB.GetComponent<FixedJoint>().breakTorque = 2000f;
            }
             
            }
        }
       
    }

