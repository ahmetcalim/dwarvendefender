using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WeaponThrowingManager : MonoBehaviour
{
    public Rigidbody weaponRB;
    public Transform backPos;
    public bool isThrowing;
    public Animator handAnimator;
    public SteamVR_Action_Pose _handActionPose;
    public SteamVR_Behaviour_Pose _handBehaviourPose;
    public bool isRotating;
    public float weaponVelocity;
    public float velocityMultiplier;
    public SteamVR_Input_Sources sourceHand;
    private float multip;
    public bool hasEffect;
    public GameObject basePoseBehaviour;
    private void Start()
    {
        if (weaponRB.GetComponent<Weapon>().throwable)
        {
            handAnimator.SetTrigger("holding_weapon");
            hasEffect = weaponRB.GetComponent<Weapon>().hasEffect;
        }
      
    }
    public void Throw()
    {
        if (!isThrowing && weaponRB && weaponRB.GetComponent<Weapon>().throwable)
        {

            Vector3 handVelociy = _handBehaviourPose.GetVelocity();

            weaponRB.GetComponent<Weapon>().inHand = false;
            if (weaponRB.GetComponentInChildren<Stabber>())
            {
                weaponRB.GetComponentInChildren<Stabber>().comingBack = false;
            }
            weaponRB.GetComponent<Weapon>().weaponEffect.SetActive(true);
            weaponRB.constraints = RigidbodyConstraints.None;
            weaponRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            weaponRB.transform.SetParent(null);
          
            backPos.GetComponent<BoxCollider>().enabled = false;
            Vector3 velocityDirection = _handBehaviourPose.GetVelocity().normalized;
            weaponRB.velocity = velocityDirection * handVelociy.magnitude * 3f;
            weaponRB.angularVelocity = _handBehaviourPose.GetAngularVelocity();
            isThrowing = true;
            handAnimator.SetTrigger("idle");
        }
    }
    void Update()
    {
        if (weaponRB.velocity.magnitude > 2)
        {
            weaponVelocity = weaponRB.velocity.magnitude;
            weaponRB.transform.rotation.SetLookRotation(-weaponRB.velocity.normalized);
        }
    }
    private void SetVelocityToWeapon(Vector3 direction, int damageLevel)
    {
      
    }
    private float GetVelocityByDamageLevel(int index)
    {
        return _handActionPose.GetLastVelocity(sourceHand).magnitude * (Mathf.Pow(weaponRB.GetComponent<Weapon>().damageMultipliers[4], 2));
    }
    public void ComeBack()
    {
        if (isThrowing)
        {
            if (weaponRB.transform.parent != backPos.GetComponent<WeaponBaseBehaviour>().parent)
            {
                float distance = Vector3.Distance(transform.position, weaponRB.transform.position);
                if (weaponRB.GetComponent<FixedJoint>())
                {
                    Destroy(weaponRB.GetComponent<FixedJoint>());
                }
                if (!backPos.GetComponent<BoxCollider>().enabled)
                {
                    handAnimator.SetTrigger("waiting_weapon");
                    multip = ((distance - 20) / 100 + 1) * velocityMultiplier / 20f;
                    backPos.GetComponent<BoxCollider>().enabled = true;
                }

                Vector3 dir = -(weaponRB.transform.position - backPos.transform.position).normalized;
                weaponRB.transform.Rotate(Vector3.down * 50f);
                weaponRB.velocity = dir * multip * 10f;
            }
          

        }
    }
    
}
