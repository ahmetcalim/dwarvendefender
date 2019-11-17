using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public static bool followActive;
    public Transform parent;
 
    // Update is called once per frame
    void Update()
    {
        if (followActive)
        {

            //Camera.main.transform.SetParent(null, false);
            //Camera.main.transform.SetParent(parent,false);
           // transform.position = Camera.main.transform.position;
        }
        else
        {
           // Camera.main.transform.SetParent(null, false);
            //Camera.main.transform.SetParent(transform, false);
        }
    }
   
}
