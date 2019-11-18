using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
public class Weapon : MonoBehaviour
{
      
    public VRControllerHandler controllerHandler;
    public float Damage, Range;
    public float baseDamage = 4f;
    public float[] damageMultipliers = new float[5];
    public Vector3 basePose;
    public bool inHand;
    public Vector3 baseRotation;
    public Vector3 rotateDirection;
    public AudioSource collisionAudioSource;
    public bool collisionAudio;
    private bool canPlayCollisionAudio = true;
    public List<AudioClip> metalOnMetalClips;
    public int index;
    public enum DamageLevel {MIN, LOW, MED, HIGH, MAX}
    public DamageLevel damageLevel;
    public int DamageLevelIndex;
    public float speed;
    public int upgradeIndex;
    public Skill[] Skills = new Skill[3]; // Initialize these specifically with FindObjectOfType<SkillName>()
    public Upgrade[] Upgrades = new Upgrade[6]; // Initialize with UpgradeManager.upgradeManager.GetUpgradesByIndex(typeIndex)
    public bool WeaponUnlocked = true;
    public int WeaponCost = 500;
    public Texture WeaponSprite;
    public string WeaponName;
    public string WeaponDescription;

    public bool hasEffect;
    public GameObject weaponEffect;
    public MeshRenderer gemMesh;
    public bool throwable;
    public bool EnemyWeapon = false;
    private void Start()
    {
        if (gemMesh)
        {
            gemMesh.sharedMaterial.SetFloat("_EmissionValue", 1f);
        }
    }
    public virtual void CollisionEnter(Collision collision)
    {     
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            if (item.gameObject == collision.gameObject)
            { 
               
                return;
            }
        }
        if (collision.gameObject.GetComponent<Weapon>())
        {
            if (collisionAudio && canPlayCollisionAudio)
            {
                FindObjectOfType<GenericFunctions>().ExecuteHaptic(controllerHandler.handType);
                StartCoroutine(SetAudioSourceActive());
            }

        }
    }
    private void Update()
    {
        speed = GetComponent<Rigidbody>().velocity.magnitude;
    }
    IEnumerator SetAudioSourceActive()
    {
        canPlayCollisionAudio = false;
        yield return new WaitForSeconds(.25f);
        canPlayCollisionAudio = true;
    }
}