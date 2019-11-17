using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Action/Attack")]
public class AttackAction : AIAction
{
    public override void Act(AIStateMachine fsm)
    {
        GameObject target = fsm.Targets[fsm.ActiveTarget];
        if (target == null) return;
        if (!fsm.GetComponent<NavMeshAgent>().isActiveAndEnabled) return;
        if (!fsm.GetComponent<NavMeshAgent>().isStopped) fsm.GetComponent<NavMeshAgent>().isStopped = true; // Keep the AI still.
        if (!fsm.GetComponent<Mob>()) return;
     
        if (!fsm.GetComponent<Mob>().attacking) // if not attacking, start attack.
        {
            fsm.GetComponent<Mob>().Attack();
        }

    }
}
