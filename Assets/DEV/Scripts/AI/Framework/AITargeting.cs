using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITargeting : ScriptableObject {

    public AIState[] TargetStates;
    public bool NotInTargetStates;
    public string[] TypeNames;
    public abstract GameObject Scan(AIStateMachine fsm);

    public bool StateControl(GameObject go)
    {
        if (TargetStates == null || TargetStates.Length == 0) return true;

        AIStateMachine t = go.GetComponent<AIStateMachine>();
        if (!t) return NotInTargetStates;
        foreach (AIState s in TargetStates)
        {
            if (t.ActiveState == s) return !NotInTargetStates;
        }
        return NotInTargetStates;
    }
}
