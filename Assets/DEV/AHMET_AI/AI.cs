using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Transform currentTarget;
    public float attackRange;
    public float outOfRange;
    public float updateTick;
    public float distance;
    private Mob mobClass;
    public Transform awayTarget;
    public bool InSight;
    public LayerMask mask;
    public int percentageOfBeingRunner;
    public int percentageOfBeingChampion;
    public Vector3 HeadOffset = new Vector3(0, 1, 0);
    public bool puppetmastertest;
    private void OnEnable()
    {
        if (!mobClass)
        {
            mobClass = GetComponent<Mob>();
        }
        if (mobClass.sliced || mobClass.dead) return;
        NavMesh.avoidancePredictionTime = 5f;
        NavMesh.pathfindingIterationsPerFrame = 500;
        FindTarget();
        StartCoroutine(UpdateDestination());
    }
    private IEnumerator UpdateDestination()
    {
        yield return new WaitForSeconds(updateTick);
        if (currentTarget)
        {
            if (!mobClass.target)
            {
                mobClass.target = currentTarget;
            }
            distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(currentTarget.position.x, currentTarget.position.z));
           
            if (distance > outOfRange)
            {
                if (mobClass.attackType == Mob.AttackType.MELEE)
                {
                    MoveCurrentTarget();
                }
            }
            else
            {
                if (distance < attackRange)
                {
                    if (mobClass.attackType == Mob.AttackType.MELEE)
                    {
                        Attack();
                    }
                }
            }
          
        }
        StartCoroutine(UpdateDestination());
    }

    private void MoveCurrentTarget()
    {
        mobClass.StopAttacking();
        if (mobClass.attackType == Mob.AttackType.RANGE)
        {
            mobClass.GetComponent<Archer>().MoveTowardsTarget(currentTarget);
        }
        else
        {
            mobClass.Move(currentTarget.position);
        }
    }

    private void Attack()
    {
        mobClass.Stop();
        if (mobClass.navMeshAgent)
        {
            if (mobClass.navMeshAgent.isOnNavMesh)
            {
                if (!mobClass.navMeshAgent.isStopped)
                {
                    mobClass.navMeshAgent.isStopped = true;
                }
            }
        }
        mobClass.Look(currentTarget.position);
        if (mobClass.attackType == Mob.AttackType.RANGE)
        {
            mobClass.GetComponent<Archer>().AttackPerSecond();
        }
        else
        {
            mobClass.Attack();
        }
       
    }

    public void FindTarget()
    {
        int percentage = Random.Range(0, 100);
        if (percentageOfBeingRunner > percentageOfBeingChampion)
        {
            if (percentage <= percentageOfBeingRunner)
            {
                //Spire a git
                if (mobClass.attackType == Mob.AttackType.MELEE)
                {
                    if (MobSpawn.onSpire < SpawnRules.maxOnSpire)
                    {
                        currentTarget = FindObjectOfType<Spire>().transform;
                        if (currentTarget)
                        {
                            MoveCurrentTarget();
                        }
                    }
                    else
                    {
                        //Player a git
                        currentTarget = Camera.main.transform;
                        if (currentTarget)
                        {
                            if (mobClass.attackType == Mob.AttackType.MELEE)
                            {
                                MoveCurrentTarget();
                            }
                            else
                            {
                                GetComponent<Mob>().target = currentTarget;
                                Attack();
                            }
                        }
                    }

                }
                else
                {

                    currentTarget = FindObjectOfType<Spire>().transform;
                    if (currentTarget)
                    {
                        GetComponent<Mob>().target = currentTarget;
                        Attack();

                    }
                }


            }
            else
            {
                //Player a git
                currentTarget = Camera.main.transform;
                if (currentTarget)
                {
                    if (mobClass.attackType == Mob.AttackType.MELEE)
                    {
                        MoveCurrentTarget();
                    }
                    else
                    {
                        GetComponent<Mob>().target = currentTarget;
                        Attack();
                    }
                }
            }
        }
        else
        {
            if (percentage >= percentageOfBeingChampion)
            {
                if (mobClass.attackType == Mob.AttackType.MELEE)
                {
                    if (MobSpawn.onSpire < SpawnRules.maxOnSpire)
                    {
                        currentTarget = FindObjectOfType<Spire>().transform;
                        if (currentTarget)
                        {
                            MoveCurrentTarget();
                        }
                    }
                    else
                    {
                        //Player a git
                        currentTarget = Camera.main.transform;
                        if (currentTarget)
                        {
                            if (mobClass.attackType == Mob.AttackType.MELEE)
                            {
                                MoveCurrentTarget();
                            }
                            else
                            {
                                Attack();
                            }
                        }
                    }
                }
                else
                {
                    currentTarget = FindObjectOfType<Spire>().transform;
                    if (currentTarget)
                    {
                        GetComponent<Mob>().target = currentTarget;
                        Attack();
                    }
                }
                  
            }
            else
            {
                //Player a git
                currentTarget = Camera.main.transform;
                if (currentTarget)
                {
                    if (mobClass.attackType == Mob.AttackType.MELEE)
                    {
                        MoveCurrentTarget();
                    }
                    else
                    {
                        GetComponent<Mob>().target = currentTarget;
                        Attack();
                    }
                }
            }
        }
        if (currentTarget)
        {
            mobClass.target = currentTarget;
        }
    }
    private Transform AwayTarget()
    {
        // Acquiring away target.
        awayTarget.position = currentTarget.position - transform.position;
        awayTarget.position = new Vector3(awayTarget.position.x, transform.position.y, awayTarget.position.z);
        NavMeshHit nHit;
        NavMesh.SamplePosition(awayTarget.position, out nHit, 10, NavMesh.AllAreas);
        awayTarget.position = nHit.position;
        return awayTarget;
    }
    private bool TargetInSight()
    {
        // Prepare raycast.
        Vector3 SpotterPos = transform.position + HeadOffset;
        Transform target = currentTarget;

        RaycastHit hit;
        Physics.Raycast(SpotterPos, target.position - SpotterPos, out hit, Mathf.Infinity, mask);

        if (hit.collider)
        {
            Debug.Log(hit.collider.tag);

            if (hit.collider.CompareTag("Spire") || hit.collider.CompareTag("Player") || hit.collider.CompareTag("MainCamera"))
            {

                Debug.DrawLine(target.position, SpotterPos, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(target.position, SpotterPos, Color.red);
                return false;
            }

        }
        else
        {
            return false;
        }
    }
}
