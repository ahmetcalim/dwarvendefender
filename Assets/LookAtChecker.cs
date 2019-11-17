using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtChecker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<AimScript>().rotate = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<AimScript>().rotate = true;
        }
    }
}
