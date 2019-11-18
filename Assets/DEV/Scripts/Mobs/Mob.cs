using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MagicalFX;
using RootMotion.Dynamics;
using UnityEngine.Events;

public class Mob : NewAI
{
    public UnityEvent onDeath;
    public UnityEvent onAttack;
    public UnityEvent onTakeDamage;
    public UnityEvent onTalk;
    public float hitPoint;
    public enum AttackType {MELEE}
    public AttackType attackType;
    public enum MobType {GOBLIN, TROLL, FIRE}
    public MobType mobType;
    public bool lookAt;
    public Transform root;
    public int attackVarCount;
    public Animator animator;
    protected AudioSource audioSource;
    public NavMeshAgent navMeshAgent;
    public bool dead;
    public bool sliced;
    public float DPS;
    [HideInInspector]
    public bool attacking;
    public ResourceManager resourceManager;
    private bool canMove = true;
    protected FX_FadeToGround faderToGround;
    public bool isDigger;
    public PuppetMaster puppet;
    public Transform target;
    public TMPro.TextMeshProUGUI healthDisplayer;
    public int hitCount;
    private float nextActionTime = 0.0f;
    public float period = 7f;
    public bool killedOnce;
    public int cost;
    protected virtual void SetRequireComponents()
    {
        faderToGround = GetComponent<FX_FadeToGround>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        resourceManager = FindObjectOfType<ResourceManager>();
    }
    public void Look(Vector3 targetPos)
    {
        if (lookAt)
        {
            if (sliced || dead)
            {
                return;
            }
            Vector3 targetTransform = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            Vector3 own = transform.position;
            root.transform.rotation = Quaternion.LookRotation(-(own - targetTransform));
        }
   
    }
   
    public virtual void TakeDamage(float damageAmount)
    {
        if (ComboTracker.comboTracker)
            damageAmount *= (1 + ComboTracker.comboTracker.comboCounter / 10);

        hitCount += 1;
        onTakeDamage.Invoke();
        hitPoint -= damageAmount;
        if (hitPoint <= 0)
        {
            DestroySelf();
        }
    }
    IEnumerator Back()
    {
        yield return new WaitForSeconds(.1f);
     
        ActivateNavMeshAgent();
    }
    void PlayAudio(List<AudioClip> audioClips)
    {
       // audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }
    public virtual IEnumerator TakeDamageOverTime(float dPerSec, float t, float interval)
    {
        TakeDamage(dPerSec / interval);
        if (hitPoint <= 0)
        {
            if (!killedOnce)
            {
                AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillMagicalAbilities });
                killedOnce = true;
            }
        }
        yield return new WaitForSeconds(interval);
        if (t - interval > 0)
        {
            if (t > interval)
            {
                StartCoroutine(TakeDamageOverTime(dPerSec, t - interval, interval));
            }
            else
            {
                StartCoroutine(TakeDamageOverTime(dPerSec, t, t));
            }
        }
    }
    public virtual void DestroySelf()
    {
        
        if (!dead)
        {
            onDeath.Invoke();
            if (navMeshAgent)
            {
                Destroy(navMeshAgent);
            }
            if (puppet)
            {
                puppet.state = PuppetMaster.State.Dead;
            }
            
            resourceManager.AddKill(this);
           
            dead = true;
            attackPoint.isFull = false;
        }
        StartCoroutine(FadeToGround());
      
    }
    public virtual void Attack()
    {
        if (attackType != AttackType.MELEE) return;
        attacking = true;
       
        Stop();
        if (targetType == Target.Player)
        {

            Look(FindObjectOfType<PlayerHealthTracker>().transform.position);
        }
        else
        {
            Look(FindObjectOfType<Spire>().transform.position);
        }
        if (animator)
        { 
            animator.SetBool("Run", false);
            animator.SetInteger("Attack", Random.Range(1, attackVarCount));

        }
        onAttack.Invoke();
    }
    public virtual void AttackToSpire()
    {
        
        if (dead || sliced) return;
        Look(target.transform.position);
        Debug.Log("spire a damage veriyor");
        target.GetComponent<Spire>().TakeDamage(DPS);
       
      
    }
    public virtual void AttackToPlayer()
    {
        if (dead || sliced) return;
        Look(target.transform.position);
        
    }
    private IEnumerator ReactivateAnimation(bool animationActive)
    {
        
        yield return new WaitForSeconds(1F);
        canMove = true;
        ActivateNavMeshAgent();
    }
    private IEnumerator FadeToGround()
    {
        yield return new WaitForSeconds(10);
        DeactiveHipsColliders();
        faderToGround.Down = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject.GetComponentInParent<MobParent>().gameObject);
    }
    private void DeactiveHipsColliders()
    {
        if (puppet)
        {
            foreach (var item in puppet.GetComponentsInChildren<Collider>())
            {
                item.isTrigger = true;
            }
        }
    }
    public void StopAttacking()
    {
        attacking = false;
        if (animator)
        {
            if (animator.GetInteger("Attack") > 0)
                animator.SetInteger("Attack", 0);
        }
      
    }
    protected virtual void ActivateNavMeshAgent()
    {
        if (navMeshAgent)
        {
            if (!navMeshAgent.isOnNavMesh)

            {
                TakeDamage(hitPoint - 5f);
                    
                return; }
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
        }
   
    }
    protected virtual void DeactivateNavMeshAgent()
    {
        if (navMeshAgent)
        {
            if (!navMeshAgent.isOnNavMesh) return;
            navMeshAgent.isStopped = true;
        }
    }
    protected virtual void SetAgent(int state)
    {
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = state == 0;
        }
    }
    void Update()
    {

        if (navMeshAgent)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                if (!navMeshAgent.isStopped)
                {
                    animator.SetFloat("Forward", navMeshAgent.velocity.magnitude);

                }
            }
        }
     
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            onTalk.Invoke();
        }
    }
    public virtual void Move(Vector3 _TargetPos)
    {
        if (canMove)    
        {
            StopAttacking();
            if (animator)
            {
                animator.SetBool("Run", true);
            }
            
            if (navMeshAgent)
            {
                if (navMeshAgent.isOnNavMesh)
                {
                    //Look(_TargetPos);

                    navMeshAgent.SetDestination(_TargetPos);
                }
            }
          
            ActivateNavMeshAgent();
        }
    }
    public void Stop()
    {
        DeactivateNavMeshAgent();
    }

}
