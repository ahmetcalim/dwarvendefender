using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mob_Eye : MonoBehaviour
{

    public Transform target;
    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
        }
           
    }
}
