using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICondition : ScriptableObject {

    public abstract bool CheckCondition(AIStateMachine fsm);
}
