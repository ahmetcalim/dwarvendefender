using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolStab : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in GetComponents<FixedJoint>())
        {
            if (item.connectedBody == null)
            {
               item.connectedBody = collision.rigidbody;
            }
        }
      
       
       
    }
}
