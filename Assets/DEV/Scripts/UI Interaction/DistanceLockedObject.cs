using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLockedObject : MonoBehaviour
{
    public Vector3 SnapLocation;
    public float Distance;
    public Vector3 SnapLocationLocal;
    public Vector3 SnapRotationLocal;
    public bool distanceCheck = true;
    // Update is called once per frame
    void Update()
    {
        if (distanceCheck)
        {
            if (Vector3.Distance(transform.position, SnapLocation) > Distance)
            {
                if (GetComponent<Rigidbody>())
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
                transform.position = SnapLocation;
                transform.rotation = Quaternion.identity;
            }
        }

    }
}
