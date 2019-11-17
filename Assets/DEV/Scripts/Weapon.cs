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
    public IEnumerator SeekUpgrades(float t, int upgradeIndex)
    {
        this.upgradeIndex = upgradeIndex;
        if (!UpgradeManager.upgradeManager)
        {
            yield return new WaitForSeconds(t);
            StartCoroutine(SeekUpgrades(t, upgradeIndex));

        }
        else
        {
            UpgradeManager um = UpgradeManager.upgradeManager;
            Upgrades = um.GetUpgradesByIndex(upgradeIndex);
            if (um.WeaponUnlocks.Length > upgradeIndex && um.WeaponUnlocks[upgradeIndex]) WeaponUnlocked = um.WeaponUnlocks[upgradeIndex];
            ApplyAllUpgrades(upgradeIndex);
        }
    }
    public void ApplyAllUpgrades(int upgradeIndex)
    {
        int _indexStart = upgradeIndex * UpgradeManager.upgradeManager.UpgradesPerWeapon;
        ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[_indexStart]);
        ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[_indexStart + 1]);
        for (int i = 0; i < 3; i++)
        {
            if (Skills[i] != null) Skills[i].ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[_indexStart + i + 2]);
        }

        #region Ultimate Setup
        var moForces = FindObjectsOfType<MysticismOfForce>();
        foreach (MysticismOfForce m in moForces)
            m.ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[5]);

        var moMatters = FindObjectsOfType<MysticismOfMatter>();
        foreach (MysticismOfMatter m in moMatters)
            m.ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[11]);

        var aoj = FindObjectsOfType<AmbassadorOfJustice>();
        foreach (AmbassadorOfJustice a in aoj)
            a.ApplyUpgrade(UpgradeManager.upgradeManager.Upgrades[17]);
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        // Dynamic friction
    }
    public void ApplyUpgrade(Upgrade u)
    {
        if (u == null) return; // i meannnnnnnnnnn
        if (u.AffectedVariables.Length == 0) return; // if the upgrade does nothing, return.
        if (u.UpgradeLevel == 0) return; // if the upgrade, in fact, didn't upgrade, return.
        if (u.UpgradeLevel > u.UpgradeCosts.Length) return; // if level is too high, return.
        for (int i = 0; i < u.AffectedVariables.Length; i++)
        {
            for(int j = 0; j < u.UpgradeLevel; j++)
            {
                if (u.AffectedVariables[i] == UpgradeVariables.Damage)
                {
                    baseDamage += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Range)
                {
                    Range += u.UpgradeMagnitudes[i].data[j];
                }
            }
        }
        // change stuff if range is upgraded
        if (Range > 0 && GetComponent<ReachExtender>())
        {
            Transform p = transform.parent;
            transform.SetParent(null);

            GetComponent<ReachExtender>().UpdateRange(Range, transform.position);
            transform.SetParent(p);
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