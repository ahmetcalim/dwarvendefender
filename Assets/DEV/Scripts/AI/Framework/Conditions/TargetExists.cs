using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/Exists")]
public class TargetExists : AICondition
{
    public bool TargetIsNull = false;
    public override bool CheckCondition(AIStateMachine fsm)
    {
        if (fsm.Targets[fsm.ActiveTarget]) return !TargetIsNull;
        return TargetIsNull;
    }
}
