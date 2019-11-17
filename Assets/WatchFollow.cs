using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-(transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z)));
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    }
}
