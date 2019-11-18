using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponyHand : MonoBehaviour
{

    private bool colliding;
    private Collider col;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            colliding = true;
            col = collision;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            colliding = false;
            col = null;
            if (GetComponent<FixedJoint>())
            {
                Destroy(GetComponent<FixedJoint>());
            }
        }
    }
    public void SetJoint()
    {
        if (colliding)
        {
            if (col != null)
            {
                if (GetComponent<FixedJoint>())
                {
                    GetComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                }
                else
                {
                    gameObject.AddComponent<FixedJoint>().connectedBody = col.gameObject.GetComponent<Rigidbody>();
                }
            }
          
        }
      
    }

    public void RemoveJoint()
    {
        if (GetComponent<FixedJoint>())
        {
            Destroy(GetComponent<FixedJoint>());
        }
    }
}
