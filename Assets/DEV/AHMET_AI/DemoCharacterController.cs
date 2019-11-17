using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().velocity = Vector3.left *2f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody>().velocity = Vector3.back * 2f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().velocity = Vector3.right * 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody>().velocity = Vector3.forward * 2f;
        }
    }
}
