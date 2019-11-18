using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Dwarven : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public bool seeTarget;
    private RaycastHit hit;
    public float viewRange;
    public Transform head;
    public LayerMask targetLayer;
    private void Start()
    {
        head.GetComponent<AI_Mob_Eye>().target = target;
    }
    private void Update()
    {
        LookToTarget();
        GetSeeingTarget();
        UpdatePath();
    }
    private void GetSeeingTarget()
    {
        if (Physics.Raycast(head.position, head.forward, out hit, viewRange))
        {
            if (hit.transform == target)
            {
                seeTarget = true;
            }
            else
            {
                seeTarget = false;
            }
        }
        else
        {
            seeTarget = false;
        }
    }
    public void UpdatePath()
    {
        if (agent.isOnNavMesh && seeTarget)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
    private void LookToTarget()
    {
        Vector3 targetTransform = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 own = transform.position;
        transform.rotation = Quaternion.LookRotation(-(own - targetTransform));
    }


}
