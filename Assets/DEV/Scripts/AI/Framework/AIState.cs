using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class AIState : ScriptableObject {

    public AIAction[] Actions;
    public AITargeting[] Scanners;
    public AITransition Transition;

	public void UpdateState(AIStateMachine fsm)
    {
        bool retargetTick = fsm.retarget >= fsm.RetargetTime;
        for (int i = 0; i < Scanners.Length; i++)
        {
            if ((fsm.Targets[i] == null || retargetTick) && Scanners[i] != null)
            {

                if (Scanners[i].Scan(fsm).gameObject == fsm.gameObject)
                {
                    return; 
                }
                fsm.Targets[i] = Scanners[i].Scan(fsm);
            }
              
        }
        if (retargetTick)
        {
            fsm.retarget = 0;
        }
        DoActions(fsm);
        CheckTransition(fsm);
    }
    private void DoActions(AIStateMachine fsm)
    {
        bool conditionsHold;
        
        for(int i = 0; i < Actions.Length; i++) // For all actions in a state:
        {
            fsm.ActiveTarget = i;
            conditionsHold = true;
            foreach(AICondition c in Actions[i].Conditions) // Pre-action condition checks.
            {
                if (!c.CheckCondition(fsm))
                {
                    conditionsHold = false;
                    break;
                }
            }
            if (conditionsHold) // If all conditions are satisfied, the action should take place.
            { 
                Actions[i].Act(fsm);
                fsm.ActiveAction = Actions[i];
                return;
            }
        }
        fsm.ActiveAction = null;
    }

    private void CheckTransition(AIStateMachine fsm)
    {
        if (!Transition) return; // If there was no transition setup, return.
        for(int i = 0; i < Transition.Conditions.Length; i++) // Some checks should be set up for a transition attempt. 
        {
            fsm.ActiveTarget = Transition.Targets[i];
            if (!Transition.Conditions[i].CheckCondition(fsm)) return; // If any of them fail, return.
        }

        // Setup for weights.
        float weightSum = 0; 
        foreach(float w in Transition.Weights)
        {
            weightSum += w;
        }

        float rand = Random.Range(0, weightSum); // Roll the dice.
        weightSum = 0; // Reset weightSum to use as a counter.
        // Here comes the check.
        for(int i = 0; i < Transition.Weights.Length; i++)
        {
            weightSum += Transition.Weights[i]; // Keep the cumulative on the counter.
            if (rand <= weightSum) // Effectively checking the dice against the gate values.
            {
                Debug.Log("Total weight: " + weightSum.ToString());
                Debug.Log("Transitioning from " + fsm.ActiveState.ToString() + " to " + Transition.States[i].ToString());
                fsm.ActiveState = Transition.States[i];
                fsm.Targets = new GameObject[fsm.ActiveState.Scanners.Length];
                return;
            }
        }
    }
}
