using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Action/Wait")]
public class WaitAction : AIAction
{
    public bool Stationary = true;
    public override void Act(AIStateMachine fsm)
    {
        if (fsm.GetComponent<Mob>() && fsm.GetComponent<Mob>().attacking) fsm.GetComponent<Mob>().StopAttacking();
        if (fsm.GetComponent<NavMeshAgent>().isOnNavMesh)
        {
           // fsm.GetComponent<Mob>().Wait();
        }
       
    }
}
