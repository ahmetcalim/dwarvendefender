using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinStateOfficerTrigger : MonoBehaviour
{
    private bool active = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (active)
            {
                FindObjectOfType<BossDwarf>().WinState();
                active = false;
            }
           
        }
    }
}
