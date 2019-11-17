using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : ScriptableObject {

    public AICondition[] Conditions;
    public abstract void Act(AIStateMachine fsm);
}
