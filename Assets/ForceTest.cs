using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    public float massMult;
    public float force;
    public GameObject prefab;
    public void Throw()
    {
        GameObject obj =  Instantiate(prefab, transform.position, Quaternion.identity);
       // obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * (force * massMult), ForceMode.Impulse);
    }
}
