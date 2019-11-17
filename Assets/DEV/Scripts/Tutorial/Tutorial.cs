using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public static int mobTrainingCount;
    public GameObject finishGate;
    public GameObject finishGateTrigger;
    public static bool tutorialFinished;
    public UnityEvent mobsHit;
    private void Update()
    {
        if (mobTrainingCount > 10)
        {
            if (!tutorialFinished)
            {
                mobsHit.Invoke();
                finishGateTrigger.SetActive(true);
                finishGate.GetComponent<Animator>().enabled = true;
                tutorialFinished = true;
            }
            
        }
    }
}
