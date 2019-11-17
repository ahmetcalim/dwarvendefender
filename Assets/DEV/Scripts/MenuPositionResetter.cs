using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPositionResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PoseInMap>())
        {
            if (other.GetComponent<Rigidbody>())
            {

                if (other.GetComponent<Rigidbody>().velocity.magnitude > .01f)
                {
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    other.GetComponent<PoseInMap>().ResetPosition();
                }

            }
        }
    }
}
