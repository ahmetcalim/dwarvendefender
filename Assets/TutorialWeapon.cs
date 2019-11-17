using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWeapon : MonoBehaviour
{
    public bool matched;
    public GameObject grabTutorial;
    public GameObject throwingTutorial;
    private void LateUpdate()
    {
        if (matched)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<Outline>().enabled = false;
            grabTutorial.SetActive(false);
            if (throwingTutorial)
            {
                throwingTutorial.SetActive(true);
            }
           
        }
    }
}
