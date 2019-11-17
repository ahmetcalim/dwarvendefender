using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportCardHolder : MonoBehaviour
{
    private bool reportCardTaken;
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "ReportObject")
        {
            if (!reportCardTaken)
            {
                if (other.transform.parent == null)
                {
                    other.GetComponent<Animator>().enabled = false;

                    other.transform.position = transform.position;
                    FindObjectOfType<BossDwarf>().StartOfTheCoin();
                    reportCardTaken = true;
                }
              
            }
          
        }
    }
}
