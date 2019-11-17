using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Condition/Distance")]
public class TargetDistanceCondition : AICondition
{
    [Tooltip("The distance to compare with.")]
    public float Distance = 1.0f;
    [Tooltip("True if you want to check target to be closer.")]
    public bool CheckIfCloser = true;
    float currentDistance;
    public override bool CheckCondition(AIStateMachine fsm)
    {
        return DistanceControl(fsm);
    }

    private bool DistanceControl(AIStateMachine fsm)
    {
        GameObject target = fsm.Targets[fsm.ActiveTarget];
        if (target == null) {
            return false;
        }
        
        currentDistance = Vector3.Distance(new Vector3(fsm.transform.position.x, 0f, fsm.transform.position.z), new Vector3(target.transform.position.x, 0f, target.transform.position.z));
        if (CheckIfCloser &&  currentDistance < Distance) return true;

        if (!CheckIfCloser && currentDistance > Distance) return true;
        return false;
    }
}
