using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Mob
{
    public float pShoulderHeight;
    public Transform shoulderRef;
    public Transform shoulderRefBottom;
    public float ownShoulderHeight;
    public float projectileSpeed;
    private float g = 9.81f;
    public float maxAttackRange;
    private float distance;
    public float inverseExpertise;
    public Vector3 vecFromMobToPlayer;
    public Vector3 playerMovementVec;
    public Transform arrowSpawnPoint;
    public Rigidbody arrowPrefab;
    public GameObject arrowForAnim;
    private float nextShootTime = 0.0f;
    public float period = 0.1f;
    private float timer;
    private Vector3 previousPosition;
    public AudioClip bowSound;
    public AudioClip arrowRelease;
    public float maxSpeed;

    private float GetShootingAngle(float playerSpeed)
    {
        ownShoulderHeight = shoulderRef.position.y;
        pShoulderHeight = target.position.y * .85f;
        if (playerSpeed < 1.1)
        {
            float asin = Mathf.Asin((distance * g)
                           / Mathf.Pow(projectileSpeed, 2));
            float atan = Mathf.Atan((pShoulderHeight - ownShoulderHeight)
                           / distance);

            float angle = (asin / 2f) + atan;
            
            return angle;
        }
        else
        {
            return Mathf.Asin((distance * g)
                           / Mathf.Pow(projectileSpeed, 2))
                           + Mathf.Atan((pShoulderHeight - ownShoulderHeight)
                           / distance);
        }
    }
    private Vector3 Direction()
    {
        return target.position - arrowSpawnPoint.position;
    }
    public void Shoot()
    {
        if (animator.GetInteger("Attack") == 0)
        {
            animator.SetInteger("Attack", 1);
            animator.SetBool("Run", false);
        }
        
    }
    public void AttackPerSecond()
    {
        timer += Time.deltaTime;
       
        if (timer > nextShootTime)
        {
            this.target = target.transform;
            distance = Vector3.Distance(transform.position, this.target.position);
            nextShootTime += period;
            Look(target.position);
            Shoot();

            // execute block of code here
        }
    }
    public void ShootAfterAnim()
    {
        Rigidbody arrow = Instantiate(arrowPrefab.gameObject, arrowSpawnPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        arrow.GetComponent<ArrowBehaviour>().targetT = target;
        Vector3 dir = Direction().normalized;
        float angle = GetShootingAngle(.9f);
        arrow.velocity = new Vector3(dir.x, angle, dir.z) * projectileSpeed;
       
    }
    private void OnEnable()
    {
        SetRequireComponents();
    }
    public void PlayBowSound()
    {
        audioSource.PlayOneShot(bowSound);
    }
    public void PlayArrowReleaseClip()
    {
        audioSource.PlayOneShot(arrowRelease);
    }
    public void EnableArrow()
    {
        arrowForAnim.SetActive(true);
    }
    public void DisableArrow()
    {
        arrowForAnim.SetActive(false);
    }
    public void MoveTowardsTarget(Transform target)
    {
        Move(target.position);
        if (navMeshAgent)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                if (navMeshAgent.destination != target.position)
                {
                    navMeshAgent.SetDestination(target.position);
                }
            }
        }
       
    }
    //Virtual voids
    public override void Stagger()
    {
        base.Stagger();
    }
    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
    }
    public override IEnumerator TakeDamageOverTime(float dPerSec, float t, float interval)
    {
        return base.TakeDamageOverTime(dPerSec, t, interval);
    }
    protected override void SetRequireComponents()
    {
        base.SetRequireComponents();
    }
    protected override void ActivateNavMeshAgent()
    {
        base.ActivateNavMeshAgent();
    }
    protected override void DeactivateNavMeshAgent()
    {
        base.DeactivateNavMeshAgent();
    }
    protected override void SetAgent(int state)
    {
        base.SetAgent(state);
    }
    
}
