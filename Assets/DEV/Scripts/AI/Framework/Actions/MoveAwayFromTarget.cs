using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Action/Move Away")]
public class MoveAwayFromTarget : AIAction
{
    [Tooltip("The float to multiply the normalized away vector with.")]
    public float ReferenceDistance = 1.0f;
    private Vector3 awayTarget;
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

        // Acquiring away target.
        awayTarget = fsm.transform.position - target.transform.position;
        awayTarget = fsm.transform.position + awayTarget.normalized * ReferenceDistance;
        awayTarget = new Vector3(awayTarget.x, Terrain.activeTerrain.SampleHeight(awayTarget), awayTarget.z);
        NavMeshHit nHit;
        NavMesh.SamplePosition(awayTarget, out nHit, 10, NavMesh.AllAreas);
        awayTarget = nHit.position;

        if (fsm.GetComponent<NavMeshAgent>().destination != awayTarget)
        {
            bool ret = fsm.GetComponent<NavMeshAgent>().SetDestination(awayTarget);
        }
        fsm.GetComponent<Mob>().Move(awayTarget);
        fsm.GetComponent<Mob>().Look(awayTarget);
    }
}
