using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMob : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Weapon>())
        {
            Tutorial.mobTrainingCount++;
        }
    }
}
