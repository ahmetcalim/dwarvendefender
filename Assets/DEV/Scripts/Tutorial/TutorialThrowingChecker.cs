using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TutorialThrowingChecker : MonoBehaviour
{
    public int hitLeft = 4;
    public bool hit;
    public UnityEvent onThrowingCompleted;
    public Animator otherDoor;
    public GameObject abilityusing;
    public GameObject throwingTutorial;
    public List<GameObject> abilityTriggers;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (!hit)
            {
                hit = true;
                if (hitLeft > 0)
                {

                    hitLeft--;
                }
                else
                {
                    if (!otherDoor.enabled)
                    {
                        otherDoor.enabled = true;
                      
                        onThrowingCompleted.Invoke();
                        if (throwingTutorial)
                        {
                            Destroy(throwingTutorial);
                        }
                        if (!abilityTriggers[0].activeSelf)
                        {
                            foreach (var item in abilityTriggers)
                            {
                                item.SetActive(true);
                            }
                        }
                    }
                }
            }
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            hit = false;
        }
    }
}
