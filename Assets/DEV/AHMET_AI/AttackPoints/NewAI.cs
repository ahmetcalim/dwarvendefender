using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NewAI : MonoBehaviour
{
    Mob mobClass;
    Mob nearestMob;
    MobSpawn mobSpawn;
    NavMeshAgent navMesh;
    Animator animator;

    public Transform player;
    public Transform spire;
    public Transform target;
    internal Target targetType;
    public float playerDistance;
    public float spireDistance;
    public float targetDistance;
    public float perceptionDistance;
    internal AttackPoint attackPoint;
    public bool seeingTarget;
    RaycastHit hit;
    public int percentageOfBeingRunner;
    public int percentageOfBeingChampion;

    private void Start()
    {
        if (!mobClass)
        {
            mobClass = GetComponent<Mob>();
            navMesh = mobClass.navMeshAgent;
            animator = mobClass.animator;
        }
        if (mobClass.sliced || mobClass.dead) return;

        player = Camera.main.transform;
        spire = FindObjectOfType<Spire>().transform;
        mobSpawn = FindObjectOfType<MobSpawn>();
        SetDefaultTarget();
        StartCoroutine(RoutineUpdate());
    }

    private IEnumerator RoutineUpdate()
    {
        RecursiveUpdate();
        yield return new WaitForSeconds(.01f);
        StartCoroutine(RoutineUpdate());
    }
    void RecursiveUpdate()
    {
        CheckBehaviour();
        GetPlayerDistance();
        GetSpireDistance();
        GetTargetDistance();
    }

    void SetDefaultTarget()
    {
        int percentage = Random.Range(0, 100);
        if (percentageOfBeingRunner > percentageOfBeingChampion)
        {
            if (percentage <= percentageOfBeingRunner)
            {
                if (MobSpawn.onSpire < SpawnRules.maxOnSpire)
                    SetTarget(Target.Spire);
                else
                    SetTarget(Target.Player);
            }
            else
            {
                if (MobSpawn.onPlayer < SpawnRules.maxOnPlayer)
                    SetTarget(Target.Player);
                else
                    SetTarget(Target.Spire);
            }
        }
        else
        {
            if (percentage >= percentageOfBeingChampion)
            {
                if (MobSpawn.onSpire < SpawnRules.maxOnSpire)
                    SetTarget(Target.Spire);
                else
                    SetTarget(Target.Player);
            }
            else
            {
                if (MobSpawn.onPlayer < SpawnRules.maxOnPlayer)
                    SetTarget(Target.Player);
                else
                    SetTarget(Target.Spire);
            }

        }
    }

    void SetTarget(Target _targetType)
    {
        if (attackPoint != null)
            attackPoint.isFull = false;

        switch (_targetType)
        {
            case Target.Player:
                attackPoint = player.parent.GetComponentInChildren<AttackPoints>().SetAttackPoint();
                target = attackPoint.transform;
                targetType = _targetType;
                break;
            case Target.Spire:
                attackPoint = spire.GetComponentInChildren<AttackPoints>().SetAttackPoint();
                target = attackPoint.transform;
                targetType = _targetType;
                break;
            default:
                break;
        }
        mobClass.target = target;
    }

    void CheckBehaviour()
    {

        if (MobSpawn.onSpire < SpawnRules.maxOnSpire && MobSpawn.onSpire < MobSpawn.onPlayer)          
         {
            if (spireDistance < playerDistance && CheckVisualAngle(spire) && targetType != Target.Spire)
            {
                Debug.Log("Player a gidiyodum döndüm");
                SetTarget(Target.Spire);
            }                
         }
        if (MobSpawn.onPlayer < SpawnRules.maxOnPlayer && MobSpawn.onPlayer < MobSpawn.onSpire)            
        {
            if (playerDistance < spireDistance && CheckVisualAngle(player) && targetType != Target.Player)
            {
                Debug.Log("Spire a gidiyodum döndüm");
                SetTarget(Target.Player);
            }

        }


        if (GetNearestMob() != null)
         {
            if (!mobClass.attacking && GetNearestMob().attacking && GetNearestMob().targetType == Target.Player)
                if (MobSpawn.onPlayer < SpawnRules.maxOnPlayer && MobSpawn.onPlayer < MobSpawn.onSpire)
                {
                    Debug.Log("Player a gidiyorum. Arkadaşıma saldırıyorlar. Spire dolu");
                    mobClass.SetTarget(GetNearestMob().targetType);
                }
                    
         }
         

        if (targetDistance > perceptionDistance)
            mobClass.Move(target.position);
        else
            mobClass.Attack();
    }

    void GetPlayerDistance()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
    }
    void GetSpireDistance()
    {
        spireDistance = Vector3.Distance(spire.transform.position, transform.position);
    }

    Mob GetNearestMob()
    {
        Mob mMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Mob m in mobSpawn.mobsAlive)
        {
            
            if (m != this && m != null)
            {
                
                float dist = Vector3.Distance(m.transform.position, currentPos);
                if (dist < minDist && dist < 10)
                {
                    mMin = m;
                    minDist = dist;
                }
            }

        }
        nearestMob = mMin;
        return nearestMob;
    }
    void GetTargetDistance()
    {
        targetDistance = Vector3.Distance(target.transform.position, transform.position);
    }
    bool CheckVisualAngle(Transform t)
    {
        float tDistance = Vector3.Distance(t.transform.position, transform.position);
        Vector3 targetDir = t.transform.position- transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < 130 && tDistance < 10)
        {
            seeingTarget = true;
            return true;
           
        }          
        else
        {
            seeingTarget = false;
            return false;
           
        }
            
    }
    public enum Target { Player, Spire }
}
