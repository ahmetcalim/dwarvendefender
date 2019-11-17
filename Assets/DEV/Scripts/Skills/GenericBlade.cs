using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;

public class GenericBlade : MonoBehaviour
{
    public WeaponThrowingManager throwingManager;
    public static int BladeSliceID = 0;

    [Tooltip("The direction the blade is cutting. Local.")]
    public Vector3 BladeSlashDirection;
    [Tooltip("The direction the blade extends towards. Local.")]
    public Vector3 BladeSpanDirection;

    [Tooltip("The intended delay between slashes.")]
    public float SlashDelay;
    private float _slashTime; // Keeps track of time left until next slash is available.
    public bool isInHand;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        if (SlashDelay < 0) SlashDelay = 0; // Make sure delay is non-negative.
        _slashTime = 0; // Initialize tracker.
    }
    public SteamVR_Behaviour_Pose rb;
    public SteamVR_Input_Sources source;
    private Weapon weapon;
    public SteamVR_Action_Pose poseAction;
    public AudioSource audioSource;
    public List<AudioClip> slashClips;
    private int sliceCount;
    public int maxSliceCount = 5;
    // Update is called once per frame
    void Update()
    {
        weapon = GetComponentInParent<Weapon>();
        if (_slashTime > 0)
        {
            _slashTime -= Time.deltaTime;
            if (_slashTime < 0) _slashTime = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (throwingManager)
        {
            isInHand = !throwingManager.isThrowing;
        }
        if (GetComponent<RetributionWave>())
        {
            if (other.GetComponent<EnemySlashableComponent>())
            {
                Slice(other);
            }
           
        }
        if (isInHand)
        {
            if (!rb)
            {
                rb = GetComponentInParent<SteamVR_Behaviour_Pose>();
            }
            if (other.GetComponent<EnemySlashableComponent>() != null)
            {
               
                if (weapon.DamageLevelIndex > 2)
                {
                    if (other.GetComponent<EnemySlashableComponent>() != null)
                    {
                        Slice(other);

                    }
                }
                else
                {
                }
            }
        }
        else
        {
            if (other.GetComponent<EnemySlashableComponent>() != null)
            {

                Slice(other);
            }
        }
    }
    private Vector3 GetDirection()
    {
        Vector3 dir = -new Vector3(poseAction.GetLastVelocity(source).x, -poseAction.GetLastVelocity(source).y, poseAction.GetLastVelocity(source).z);
        return dir;
    }
 
    public void Slice(Collider other)
    {
      
        Debug.Log("KESMEYE GİRDİ");
        if (!other.GetComponent<EnemySlashableComponent>())

        {
            Debug.Log("COMPONENT YOK");
            return; }
        Debug.Log("COMPONENT VAR");
        _slashTime = SlashDelay;
        if (!GetComponent<RetributionWave>())
        {
            StartCoroutine(FindObjectOfType<GenericFunctions>().ExecuteHaptic(source));
        }
        
        EnemySlashHandler sliceable = other.GetComponent<EnemySlashableComponent>().SlashHandler; // Got the sliceable.

        // DO YOUR CONTROLS HERE. IF THEY FAIL, RETURN.
        if (!sliceable) return;
        if (sliceable.mob != null)
        {
            sliceable.mob.sliceCount++;
            if (sliceable.mob.sliceCount > maxSliceCount) return;
          
            if (!sliceable.mob.dead && !sliceable.mob.sliced)
            {
               
                if (MobSpawn.objectCount > 0)
                {
                    MobSpawn.objectCount--;
                    if (sliceable.mob.attackType == Mob.AttackType.MELEE)
                    {
                       
                    }
                    else
                    {
                        sliceable.mob.GetComponent<Archer_AI>().spawner.available = true;
                    }
                    

                    Camera.main.transform.GetComponent<Animator>().SetTrigger("ShakeCamera");
                }
            }
           // sliceable.mob.GetComponent<AI>().enabled = false;
            if (sliceable.mob.navMeshAgent)
            {
                Destroy(sliceable.mob.navMeshAgent);
            }
            sliceable.mob.sliced = true;
            sliceable.mob.dead = true;
            
          
            // Translate vectors to global for calculation.
            Vector3 _slash = transform.TransformVector(BladeSlashDirection);
            Vector3 _span = transform.TransformVector(BladeSpanDirection);
            Debug.Log("BURALARA KADAR GELDİN KES ARTIK DİMİ");


            var contactPoint = other.ClosestPoint(transform.position); // Got the contact point.
            var plane = new Plane(contactPoint, contactPoint + _slash, contactPoint + _span); // Define 3 points for the slice plane.
            var sliceID = BladeSliceID; // Grab the sliceID.
            BladeSliceID++; // Restock the sliceID like a good boy.
            sliceable.Slice(plane, BladeSliceID, null); // then just slice it fam

            _slashTime = SlashDelay; // Reset the slash delay.
        }
     
    }
}
