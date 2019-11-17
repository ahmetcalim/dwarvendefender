using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Condition/Sight")]
public class TargetInSight : AICondition
{
    public LayerMask mask;
    public Vector3 HeadOffset = new Vector3(0,1,0);
    public bool NotInSight = false;
    public override bool CheckCondition(AIStateMachine fsm)
    {
        // Prepare raycast.
        Vector3 SpotterPos = fsm.transform.position + HeadOffset;
        Transform target = fsm.Targets[fsm.ActiveTarget].transform;

        if(NotInSight)
            Debug.DrawLine(target.position, SpotterPos, Color.red);
        else
            Debug.DrawLine(target.position, SpotterPos, Color.green);

        RaycastHit hit;
        Physics.Raycast(SpotterPos, target.position - SpotterPos, out hit, Mathf.Infinity, mask);

        while (target.parent != null) target = target.parent; // go to the root of the target.

        
        List<Collider> AllColliders = new List<Collider>();
        AllColliders.AddRange(target.GetComponents<Collider>());
        AllColliders.AddRange(target.GetComponentsInChildren<Collider>());
        if (AllColliders.Contains(hit.collider)) return !NotInSight;
        
        return NotInSight;
    }

}
