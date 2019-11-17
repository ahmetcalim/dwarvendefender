using CrazyMinnow.SALSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DwarfHelperAI : MonoBehaviour
{
    Salsa3D salsa;
    AudioSource audioSource;
    NavMeshAgent navMeshAgent;
    Animator animator;
    Transform player;
    public List<DwarfHelperTriggerPoint> helperPoints;
    public UnityEvent TutorialEnd;
    public DwarfHelperTriggerPoint currentHelperPoint;
    int currentIndex;
    float playerDistance;
    float currentHelperPointDistance;
    public AnimationClip[] animationClips;

    public List<UnityEvent> onplays;
    void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, currentHelperPoint.transform.position);
        currentHelperPointDistance = Vector3.Distance(currentHelperPoint.transform.position, transform.position);

        if (currentHelperPointDistance > 1)
            Walk(currentHelperPoint.transform.position);
        else
        {
            Wait();
            Vector3 targetTransform = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            Vector3 own = transform.position;
            transform.rotation = Quaternion.LookRotation(-(own - targetTransform));
        }
           

    }

    void Start()
    {
        if (GetComponent<Salsa3D>() != null)
            salsa = GetComponent<Salsa3D>();
        if (GetComponent<AudioSource>() != null)
            audioSource = GetComponent<AudioSource>();
        if (GetComponent<NavMeshAgent>() != null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();

        player = FindObjectOfType<PlayerHealthTracker>().transform;
        currentHelperPoint = helperPoints[currentIndex];

    }

    public void SetCurrentComplete()
    {
        currentHelperPoint.Complete();
    }

    internal void Next()
    {

        currentIndex++;
        if (currentIndex < helperPoints.Count)
        {
            currentHelperPoint = helperPoints[currentIndex];
        }
        else
        {
            Debug.Log("Tutorial End!");
            End();
        }
    }

    IEnumerator TalkRoutine()
    {
      
        salsa.SetAudioClip(currentHelperPoint.clip);
        yield return new WaitForSeconds(1);

        StartCoroutine(Talk());
    }
    public IEnumerator Congratulations()
    {
        float time;
        salsa.Stop();
        int state = Random.Range(1, 3);
        if (state == 1)
            time = 1.13f;
        else
            time = 2.2f;

        animator.SetInteger("congrat", state);
        yield return new WaitForSeconds(time);
        animator.SetInteger("congrat", 0);
        Next();
    }
    IEnumerator Talk()
    {

        salsa.Play();

        animator.SetInteger("talking", Random.Range(1, 4));
        yield return new WaitForSeconds(salsa.audioClip.length);
        animator.SetInteger("talking", 0);
        StartCoroutine(WaitRoutine());
        onplays[currentIndex].Invoke();

    }
    bool isWait = true;
    void Wait()
    {
        if (!isWait)
        {
            Stop();
            isWait = true;
            if (currentHelperPoint.clip)
            {
                StartCoroutine(TalkRoutine());
            }
           
        }

    }
    IEnumerator WaitRoutine()
    {
        if (isWait)
        {         
            yield return new WaitForSeconds(1);
            Debug.Log("idle animasyonu değiştir");
        }
    }

    int GetRandomState(int min, int max, int value)
    {
        int result = Random.Range(min, max);
        if (result != value)
            return result;
        else
            return GetRandomState(min, max, value);
    }

    void Walk(Vector3 _TargetPos)
    {
        if (isWait)
        {
            animator.SetInteger("idleState", 0);
            animator.SetInteger("idle", 0);
            isWait = false;
            if (animator)
                animator.SetBool("walk", true);

            if (navMeshAgent)
                if (navMeshAgent.isOnNavMesh)
                    navMeshAgent.SetDestination(_TargetPos);
            ActivateNavMeshAgent();
        }
    }

    void Stop()
    {
        if (animator)
            animator.SetBool("walk", false);
        DeactivateNavMeshAgent();
    }

    void DeactivateNavMeshAgent()
    {
        if (navMeshAgent)
        {
            if (!navMeshAgent.isOnNavMesh)
                return;

            navMeshAgent.isStopped = true;
        }
    }

    void ActivateNavMeshAgent()
    {
        if (navMeshAgent)
        {
            if (!navMeshAgent.isOnNavMesh)
                return;
            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;
        }
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1.5f);
        TutorialEnd.Invoke();

    }
}
