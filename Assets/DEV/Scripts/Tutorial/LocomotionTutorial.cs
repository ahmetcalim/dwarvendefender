using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionTutorial : MonoBehaviour
{
    public LocomotionRules leftLoco;
    public LocomotionRules rightLoco;
    private float timer = 0.0f;
    private TutorialStateChacker chacker;

    public bool pressBothGrips;
    public bool walked { get; set; }
    public bool direction { get; set; }
    public void PressBothGrips(TutorialStateChacker tutorialStateChacker)
    {
        chacker = tutorialStateChacker;
    }
    public void SwingYourHands(TutorialStateChacker tutorialStateChacker)
    {
        chacker = tutorialStateChacker;
    }

    private void Update()
    {
        if (rightLoco.active && leftLoco.active)
        {
            pressBothGrips = true;
            if (chacker)
            {
                if (chacker.loadingBar)
                {
                    chacker.loadingBar.LoadImage(timer / 3f);
                    if (timer < 3f)
                    {
                        timer += Time.deltaTime;

                    }
                    else
                    {
                        timer = 0.0f;
                        chacker.completed.Invoke();
                    }

                }


            }
            else
            {
            }

        }
        else
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                chacker.loadingBar.LoadImage(timer / 3f);
            }

        }


    }
}
