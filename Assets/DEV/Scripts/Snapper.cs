using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DistanceLockedObject>())
        {
            if (other.gameObject.name == "MapObject")
            {
                if (other.transform.parent != this.transform)
                {
                    other.transform.parent = transform;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.transform.localPosition = other.GetComponent<DistanceLockedObject>().SnapLocationLocal;
                    other.transform.localEulerAngles = other.GetComponent<DistanceLockedObject>().SnapRotationLocal;
                  
                }
                
            }
        }
    }
}
