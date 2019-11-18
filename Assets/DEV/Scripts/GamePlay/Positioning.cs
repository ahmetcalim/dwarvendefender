using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioning : MonoBehaviour
{
    void Start()
    {
        GetComponent<BoxCollider>().center = Camera.main.transform.position;
    }
}
