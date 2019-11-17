using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/State")]
public class TargetInState : AICondition
{
    public bool NotInStates;
    public AIState[] States;
    public override bool CheckCondition(AIStateMachine fsm)
    {
        GameObject target = fsm.Targets[fsm.ActiveTarget];
        if (!NotInStates)
        {
            if (!target.GetComponent<AIStateMachine>()) return false; // This target cannot be in any state.
            AIState targetState = target.GetComponent<AIStateMachine>().ActiveState;
            for(int i = 0; i < States.Length; i++)
            {
                if (targetState == States[i]) return true;
            }
            return false;
        }
        else
        {
            if (!target.GetComponent<AIStateMachine>()) return true; // This target cannot be in any state, so it's not in this state.
            AIState targetState = target.GetComponent<AIStateMachine>().ActiveState;
            for (int i = 0; i < States.Length; i++)
            {
                if (targetState == States[i]) return false;
            }
            return true;
        }
    }
}
