using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TutorialGrab : MonoBehaviour
{
    public TutorialGrab other;
    public TutorialStateChacker tutorialStateChacker;
    public GameObject collidingObject;
    public bool grabbed;
    public bool colliding;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PoseInHand>())
        {
            if (collidingObject == null || other.gameObject == collidingObject)
            {
                if (!other.GetComponent<TutorialWeapon>().matched)
                {
                    other.GetComponent<TutorialWeapon>().matched = true;
                    collidingObject = other.gameObject;
                   
                }
                colliding = true;
                if (collidingObject)
                {
                    if (collidingObject.GetComponent<Rigidbody>())
                    {
                        if (collidingObject.GetComponent<Rigidbody>().velocity.magnitude > 5f)
                        {
                            Snap();
                        }
                    }
                }
         
               
            }
        
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (collidingObject != null)
        {
            //  collidingObject = null;
            colliding = false;
        }
    }
    public void Grab()
    {
        if (collidingObject)
        {
            if (colliding)
            {
                collidingObject.transform.parent = this.transform;
                collidingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                switch (GetComponent<VRControllerHandler>().handType)
                {
                    case SteamVR_Input_Sources.LeftHand:
                        collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                        collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                        break;
                    case SteamVR_Input_Sources.RightHand:
                        collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                        collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                        break;
                    default:
                        break;
                }
                if (!grabbed && other.grabbed)
                {
                    grabbed = true;
                    other.grabbed = true;
                    tutorialStateChacker.Complete();
                }
            }
           
        }
       
    }
    public void Snap()
    {
        if (collidingObject)
        {
            collidingObject.transform.parent = this.transform;
            collidingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            switch (GetComponent<VRControllerHandler>().handType)
            {
                case SteamVR_Input_Sources.LeftHand:
                    collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                    collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                    break;
                case SteamVR_Input_Sources.RightHand:
                    collidingObject.transform.localPosition = collidingObject.GetComponent<PoseInHand>().positionInLeftHand;
                    collidingObject.transform.localEulerAngles = collidingObject.GetComponent<PoseInHand>().rotationInLeftHand;
                    break;
                default:
                    break;
            }
        }
    }
    public void ComeBack()
    {
        if (!colliding)
        {
            if (collidingObject)
            {
                collidingObject.GetComponent<Rigidbody>().velocity = -(collidingObject.transform.position - transform.position).normalized * 20f;
            }

        }
      
    }
    public void Release()
    {
        if (collidingObject != null)
        {
            collidingObject.transform.parent = null;

            collidingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            collidingObject.GetComponent<Rigidbody>().velocity = GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().normalized * 5f * GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude;

            if (colliding)
            {
                if (collidingObject.GetComponent<Rigidbody>())
                {
                
                }
            }
          
        }
        else
        {
            Debug.Log("RELEASE BÖLÜMÜNDE COLLIDING OBJECT YOK");
        }
    }
}
