using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Action/Move Towards")]
public class MoveTowardsTarget : AIAction
{
    Vector3 targetPos;
    public override void Act(AIStateMachine fsm)
    {
        GameObject target = fsm.Targets[fsm.ActiveTarget];

        if (fsm.GetComponent<Mob>() && fsm.GetComponent<Mob>().attacking) fsm.GetComponent<Mob>().StopAttacking();
        if (target == null) return;
        if (!fsm.GetComponent<NavMeshAgent>().isActiveAndEnabled) return;
        if (fsm.GetComponent<NavMeshAgent>().isStopped) fsm.GetComponent<NavMeshAgent>().isStopped = false; // Let the AI move.
        if (fsm.GetComponent<NavMeshAgent>().pathPending)
        {
            return;
        }

        
        if (target.GetComponent<Collider>())
        {
            targetPos = target.GetComponent<Collider>().ClosestPointOnBounds(fsm.transform.position);
        }
        else if (target.GetComponentInChildren<Collider>())
        {
            targetPos = target.GetComponentInChildren<Collider>().ClosestPointOnBounds(fsm.transform.position);
        }
        else
        {
            targetPos = target.transform.position;
        }

        if (fsm.GetComponent<NavMeshAgent>().destination != targetPos)
        {
           
            bool ret = fsm.GetComponent<NavMeshAgent>().SetDestination(targetPos);
        }
        fsm.GetComponent<Mob>().Move(targetPos);
        fsm.GetComponent<Mob>().Look(targetPos);
    }
}
