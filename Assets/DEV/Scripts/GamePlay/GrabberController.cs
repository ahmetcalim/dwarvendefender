using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.Extras;

public class GrabberController : MonoBehaviour {

    //public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    // public SteamVR_Action_Boolean grabAction;
    public GrabberController otherHand;

    public GameObject collidingObject;
    public GameObject heldObject;

    public UnityEvent UIGrabbed;
    public UnityEvent UIReleased;
    public Animator handAnimator;
    public Animator otherHandAnimator;
    public GameObject[] Models;
    public Vector3 grabbedPos;
    public Vector3 grabbedRot;
    public bool hasWeapon;
    private bool reportObjectTaken;
    private void SetCollidingObject(Collider col)
    {
        if (collidingObject) return;
        if(col.gameObject.tag == "grabbable" || col.gameObject.tag == "grabbableUI" || col.gameObject.tag == "fuel")
        {
            if (col.GetComponent<Rigidbody>()) collidingObject = col.gameObject;
        }
        if (col.tag == "Coin")
        {
            col.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);       
    }

    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!collidingObject) return;
        if(other.gameObject == collidingObject) collidingObject = null;
    }

    public void Grab()
    {
        if (collidingObject != null)
        {
            if (collidingObject.GetComponent<Weapon>())
            {
                handAnimator.SetTrigger("holding_weapon");
                collidingObject.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("HOLDED WEAPON");
                collidingObject.transform.SetParent(transform);
                collidingObject.GetComponent<WeaponSelector>().SelectWeapon(GetComponent<VRControllerHandler>().index);
                hasWeapon = true;
                
                switch (GetComponent<SteamVR_Behaviour_Pose>().inputSource)
                {
                    case SteamVR_Input_Sources.LeftHand:
                        collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                        collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                        break;
                    case SteamVR_Input_Sources.RightHand:
                        collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInRightHand;
                        collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInRightHand;
                        break;
                    default:
                        break;
                }
            }
            else if (collidingObject.tag == "grabbable" || collidingObject.gameObject.tag == "fuel")
            {
                Debug.Log("BURAYA DA GİRDİ");
                if (collidingObject.GetComponent<Weapon>()) return;
                heldObject = collidingObject;
                collidingObject = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                var j = AddFixedJoint();
                j.connectedBody = heldObject.GetComponent<Rigidbody>();
            }
            if (collidingObject)
            {
                if (collidingObject == otherHand.heldObject)
                {
                    if (collidingObject.GetComponent<Weapon>())
                    {

                    }
                    else
                    {
                        Debug.Log("BURAYA DA GİRDİ");
                        if (collidingObject.tag == "grabbableUI")
                            otherHandAnimator.SetTrigger("paper");


                        otherHand.Release();
                    }


                }
              
                if (collidingObject.tag == "grabbableUI")
                {
                    Debug.Log("BURAYA DA GİRDİ");
                    if (handAnimator)
                    {
                        Debug.Log("BURAYA DA GİRDİ");
                        handAnimator.SetTrigger("paper");
                        otherHandAnimator.SetTrigger("pointer");
                    }

                }

                if (collidingObject.tag == "grabbableUI")
                {
                    if (collidingObject.GetComponent<Weapon>()) return;
                    
                    collidingObject.GetComponent<Rigidbody>().isKinematic = true;

                    if (collidingObject.name == "ReportObject")
                    {
                        if (!reportObjectTaken)
                        {
                            collidingObject.GetComponent<Animator>().enabled = false;
                            FindObjectOfType<ReportCardHolder>().GetComponent<BoxCollider>().enabled = true;
                            reportObjectTaken = true;
                        }
                       
                    }
                  
                    collidingObject.transform.SetParent(transform);
                    if (collidingObject.GetComponent<PoseInHand>())
                    {
                        switch (GetComponent<SteamVR_Behaviour_Pose>().inputSource)
                        {
                            case SteamVR_Input_Sources.LeftHand:
                                collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                                collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                                break;
                            case SteamVR_Input_Sources.RightHand:
                                collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInRightHand;
                                collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInRightHand;
                                break;
                            default:
                                break;
                        }
                    }
                
                    if (!heldObject)
                    {
                        heldObject = collidingObject;
                    }

                    collidingObject = null;


                }

            }
            if (heldObject)
            {
                if (heldObject == otherHand.heldObject)
                {
                    
                    otherHand.Release();
                }
            }
           
          
          
        

            
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void Release()
    {

        if (collidingObject)
        {
            handAnimator.SetTrigger("idle");
            collidingObject.transform.SetParent(null);
            collidingObject.GetComponent<Rigidbody>().isKinematic = false;

            collidingObject.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity() * 3f;
            collidingObject.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
            if (collidingObject.tag == "grabbableUI")
            {
                if (handAnimator)
                {
                    otherHandAnimator.SetTrigger("idle");
                }

            }
            collidingObject = null;

        }
        if (heldObject && heldObject.tag == "grabbableUI")
        {
            if (heldObject.GetComponent<Weapon>())
            {

                return;
            }
            else
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.SetParent(null);
                heldObject = null;

                handAnimator.SetTrigger("idle");
            }
           

        }
       
        if (GetComponent<FixedJoint>() && heldObject)
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }
        if (heldObject)
        {
            if (heldObject.GetComponent<Weapon>())
            {return; }
            heldObject.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            heldObject.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
            heldObject = null;
            if (handAnimator)
            {
                
                handAnimator.SetTrigger("idle");
            }
        }
        hasWeapon = false;





    }
    private void Update()
    {
        if (FindObjectOfType<MenuController>())
        {
            if (hasWeapon && otherHand.GetComponent<GrabberController>().hasWeapon)
            {
                FindObjectOfType<MenuController>().weaponsTaken = true;
            }
            else
            {
                FindObjectOfType<MenuController>().weaponsTaken = false;
            }
        }
      
    }
    // Update is called once per frame
    /*void Update () {
        if (grabAction.GetLastStateDown(handType) && collidingObject) Grab();

        if (grabAction.GetLastStateUp(handType) && heldObject) Release();
	}*/
}
