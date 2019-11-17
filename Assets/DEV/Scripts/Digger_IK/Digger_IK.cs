using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger_IK : MonoBehaviour
{
    public Transform IK_Target;
    Animator animator;
    public bool isDigging = false;
    float state = 0;
    float elapsedTimeDigging,elapsedTimeNoDigging = 0;
    public float timeReaction = 0.5f;
    public bool isRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void OnAnimatorIK()
    {
        if (isDigging)
        {

            if (isRight)
            {
                elapsedTimeNoDigging = 0;
                if (state < 1.0f)
                {
                    elapsedTimeDigging += Time.deltaTime;
                    state = Mathf.Lerp(0, 1, elapsedTimeDigging / timeReaction);

                }

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, state);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, IK_Target.position);

            }
            else
            {
                elapsedTimeNoDigging = 0;
                if (state < 1.0f)
                {
                    elapsedTimeDigging += Time.deltaTime;
                    state = Mathf.Lerp(0, 1, elapsedTimeDigging / timeReaction);

                }

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, state);
                animator.SetIKPosition(AvatarIKGoal.RightHand, IK_Target.position);
            }


        }
        else
        {
            if (isRight)
            {
                elapsedTimeDigging = 0;
                if (state > 0)
                {
                    elapsedTimeNoDigging += Time.deltaTime;
                    state = 1 - Mathf.Lerp(0, 1, elapsedTimeNoDigging / timeReaction);
                    Debug.Log(state);
                }


                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, state);
            }
            else
            {
                elapsedTimeDigging = 0;
                if (state > 0)
                {
                    elapsedTimeNoDigging += Time.deltaTime;
                    state = 1 - Mathf.Lerp(0, 1, elapsedTimeNoDigging / timeReaction);
                    Debug.Log(state);
                }


                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, state);
            }
           
        }
       
        
    }
}
