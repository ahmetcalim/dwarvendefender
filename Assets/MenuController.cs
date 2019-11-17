using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public bool weaponsTaken;
    public bool levelSelected;
    public GameObject door;
    public Animator doorAnimator;
    public GameObject mapPlaceHighlight;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
            mapPlaceHighlight.SetActive(true);
            //tutoriale yönlendirme yeri.
            PlayerPrefs.SetInt("TutorialPlayed", 0);
        }
    }
    private void Update()
    {
        if (levelSelected && weaponsTaken)
        {
            if (!door.activeInHierarchy)
            {
                door.SetActive(true);
                if (doorAnimator.GetBool("isOpen") == false)
                {
                    doorAnimator.SetBool("isOpen", true);
                }
            }
        }
        else
        {
            if (door.activeInHierarchy)
            {
                door.SetActive(false);
                if (doorAnimator.GetBool("isOpen") == true)
                {
                    doorAnimator.SetBool("isOpen", false);
                }
                
            }
        }
    }
}
