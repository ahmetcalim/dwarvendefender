using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCallObstacle : MonoBehaviour
{
    [Tooltip("Arbitrary force multiplier.")]
    public float ForceMult;
    private float Force, Damage;
    private float DeathTime = 5.0f, _t = 0;

    public void SetSkill(float f, float dmg)
    {
        Force = f; Damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<MobParent>())
        {
            var currentEnemy = other.GetComponentInParent<MobParent>().mob  ; // Kill the enemy. Tentative.

            currentEnemy.TakeDamage(Damage);

            if (currentEnemy.hitPoint <= 0)
            {
                if (!currentEnemy.killedOnce)
                {
                    AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillMagicalAbilities });
                    currentEnemy.killedOnce = true;
                }
            }
           
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * ForceMult * Force);
        }
    }
    private void Update()
    {
        // _t += Time.deltaTime;
        if (_t > DeathTime) Destroy(gameObject);
    }
}
