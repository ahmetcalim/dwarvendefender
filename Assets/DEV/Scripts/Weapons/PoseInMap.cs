using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseInMap : MonoBehaviour
{
    private Vector3 defaultPosition;


    public void ResetPosition()
    {
        transform.position = defaultPosition;
    }
    void Start()
    {
        defaultPosition = transform.position;
    }
    
}
