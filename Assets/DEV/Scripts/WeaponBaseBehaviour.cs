using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseBehaviour : MonoBehaviour
{
    public Transform parent;
    public VRControllerHandler controllerHandler;
    public string weaponTag;
    public WeaponThrowingManager throwingManager;
    public Weapon weapon;
    public Rigidbody physicsParent;
    private void Start()
    {
        if (throwingManager.weaponRB)
        {
            weapon = throwingManager.weaponRB.GetComponent<Weapon>();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Weapon>())
        {
            if (other.GetComponent<Weapon>().gameObject == throwingManager.weaponRB.gameObject)
            {
                
                
                other = other.gameObject.GetComponent<Weapon>().GetComponent<Collider>();

                if (other.transform.parent == parent)
                {
                    return;
                }
                other.transform.SetParent(parent);
                other.transform.localPosition = weapon.basePose;
                other.transform.localRotation = Quaternion.Euler(weapon.baseRotation.x, weapon.baseRotation.y, weapon.baseRotation.z);
                GetComponent<Collider>().enabled = false;
                throwingManager.handAnimator.SetTrigger("holding_weapon");

                if (other.GetComponentInChildren<Stabber>())
                {
                    other.GetComponentInChildren<Stabber>().comingBack = false;
                }
                StartCoroutine(controllerHandler.ExecuteHaptic());
                if (other.GetComponent<Hammer>())
                {
                    other.GetComponent<Hammer>().inHand = true;
                }
                other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                other.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                throwingManager.isThrowing = false;
                other.GetComponent<Weapon>().weaponEffect.SetActive(false);

            }
        }
        else
        {
            if (other.GetComponentInParent<Weapon>())
            {
                if (other.GetComponentInParent<Weapon>().gameObject != throwingManager.weaponRB.gameObject) return;
                other = other.gameObject.GetComponentInParent<Weapon>().GetComponent<Collider>();
                if (other.transform.parent == parent)
                {
                    return;
                }
                other.transform.SetParent(parent);
                other.transform.localPosition = weapon.basePose;
                other.transform.localRotation = Quaternion.Euler(weapon.baseRotation.x, weapon.baseRotation.y, weapon.baseRotation.z);
                GetComponent<Collider>().enabled = false;
                throwingManager.handAnimator.SetTrigger("holding_weapon");

                if (other.GetComponentInChildren<Stabber>())
                {
                    other.GetComponentInChildren<Stabber>().comingBack = false;
                }
                StartCoroutine(controllerHandler.ExecuteHaptic());
                other.GetComponent<Weapon>().inHand = false;
                other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                other.GetComponent<Weapon>().weaponEffect.SetActive(false);
                other.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                throwingManager.isThrowing = false;
            }
        }

    }
}
