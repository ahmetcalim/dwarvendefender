using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWeaponSelector : MonoBehaviour
{
    public TutorialWeaponSelector other;
    public TutorialStateChacker tutorialStateChacker;
    private GameObject collidingObject;
    public bool grabbed;
    private void OnTriggerEnter(Collider other)
    {
        if (!collidingObject)
        {
            if (other.GetComponent<PoseInHand>())
            {
                collidingObject = other.gameObject;
                Grab();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (collidingObject)
        {
            collidingObject = null;
        }
    }
    public void Grab()
    {
        if (collidingObject)
        {
            collidingObject.transform.position = transform.position;
            collidingObject.transform.parent = this.transform;
            collidingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (!grabbed && other.grabbed)
            {
                grabbed = true;
                other.grabbed = true;
                tutorialStateChacker.Complete();
            }
        }
    }
}
