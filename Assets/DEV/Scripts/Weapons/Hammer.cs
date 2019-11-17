using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class Hammer : Weapon
{
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources source;
  
    public SteamVR_Action_Pose rb;
    private Vector3 dir;
    public bool isHammer = true;
    private Mob hitEnemy;
    private Rigidbody hitRigidbody;
    public Rigidbody ownRigidbody;
    private bool hitting;
    private void Start()
    {
        if (isHammer)
        {
            Skills[0] = FindObjectOfType<ShatterCall>();
            Skills[1] = FindObjectOfType<EclipseSlam>();
            Skills[2] = GetComponent<ImpetusObject>();
            StartCoroutine(SeekUpgrades(0.1f, 0));
        }
    }
    public virtual void ColEnter(Collision col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            if (pose)
            {
                if (inHand)
                {
                    if (!col.gameObject.GetComponentInParent<MobParent>())
                    {
                        return;
                    }
                    hitEnemy = col.gameObject.GetComponentInParent<MobParent>().mob;
                    hitRigidbody = col.gameObject.GetComponent<Rigidbody>();
                    StartCoroutine(controllerHandler.ExecuteHaptic());
                    Damage = rb.GetLastVelocity(source).magnitude * baseDamage * damageMultipliers[DamageLevelIndex];
                    if (ComboTracker.comboTracker) Damage *= (1 + ComboTracker.comboTracker.comboCounter / 10);
                    hitEnemy.TakeDamage(Damage);
                    ApplyDirectionalForce(hitRigidbody);
                    if (hitEnemy.hitPoint <= 0 && !hitEnemy.killedOnce)
                    {
                        hitEnemy.killedOnce = true;
                        AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillBluedgeoningWeapon});
                        if (hitEnemy.hitCount == 1)
                        {
                            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.KnockDown});
                        }
                    }
                }
                else
                {
                    if (!col.gameObject.GetComponentInParent<MobParent>())
                    {
                        return;
                    }
                    hitEnemy = col.gameObject.GetComponentInParent<MobParent>().mob;

                    hitRigidbody = col.gameObject.GetComponent<Rigidbody>();
                    StartCoroutine(controllerHandler.ExecuteHaptic());
                    Damage = baseDamage * ownRigidbody.velocity.magnitude;
                    if (ComboTracker.comboTracker) Damage *= (1 + ComboTracker.comboTracker.comboCounter / 10);
                    hitEnemy.TakeDamage(Damage);
                    ApplyDirectionalForce(hitRigidbody);
                    if (hitEnemy.hitPoint <= 0 && !hitEnemy.killedOnce)
                    {
                        hitEnemy.killedOnce = true;
                        AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillBluedgeoningWeapon, Achievement.AchievementType.KillThrowingWeapon });
                        if (hitEnemy.hitCount == 1)
                        {
                            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.KnockDown });
                        }
                    }
                }
              

            }
          
        }
    }
    public virtual void ColExit(Collision col)   
    {
        if (pose && hitting)
        {
            hitting = false;
        }
    }
    private void Log(string message)
    {
        Debug.Log(gameObject.name + "_" + message + "_" + hitRigidbody.gameObject.name);
    }
    private Vector3 GetDirection()
    {
        Vector3 dir = new Vector3(rb.GetLastVelocity(source).x, rb.GetLastVelocity(source).y, rb.GetLastVelocity(source).z);
        return dir;
    }
    private void ApplyDirectionalForce(Rigidbody rbody)
    {
        dir = GetDirection();
        rbody.velocity = dir * Damage * dir.magnitude / 6f;
    }
}
