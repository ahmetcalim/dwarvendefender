using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyRotationFollower : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, transform.localPosition.y, Camera.main.transform.localPosition.z) ;
        transform.localEulerAngles = new Vector3(0f, Camera.main.transform.localEulerAngles.y, 0f);
    }
}
