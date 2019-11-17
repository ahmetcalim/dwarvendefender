using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipseSlamShockWave : MonoBehaviour
{
    [Tooltip("Arbitrary force control variable.")]
    public float ForceMult; 
    [Tooltip("Arbitrary decay control variable. Multiplies with time.")]
    public float DecayMult;

    private float _decayTime; // Fadeout time.
    private float _time; // Keeps track of active time.
    private float Diameter, Duration; // Maximum diameter and collidable duration.
    private float Force, Damage;

    void Start()
    {
        // Initialization.
        _time = 0;
        transform.localScale = new Vector3(0, 0, 0);
        _decayTime = DecayMult * Duration * 4f;
        
    }

    void Update()
    {
        _time += Time.deltaTime; // Add to active time.

        var add = (Diameter / Duration) * Time.deltaTime; // Make sphere larger.
        transform.localScale += new Vector3(1, 1, 1); 

        if (_time > _decayTime) Destroy(gameObject); // Check active duration.
        if (_time > Duration) GetComponent<Collider>().enabled = false; // Check collidable duration.

        Color c = GetComponent<MeshRenderer>().material.color; // Fade away.
        c.a = 1 - (_time / _decayTime);
        GetComponent<MeshRenderer>().material.color = c;

    }
    private void OnTriggerEnter(Collider other)
    {
        var WaveHeight = transform.position.y + transform.localScale.y * 0.2; // 20% of diameter, 40% of radius.
        if (other.ClosestPointOnBounds(transform.position).y > WaveHeight) return;
        if (other.CompareTag("enemy"))
        {
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * 10f);
            }
        }

        if (other.GetComponentInParent<MobParent>())
        {
            var currentEnemy = other.GetComponentInParent<MobParent>().mob; // Kill the enemy. Tentative.
          

            currentEnemy.TakeDamage(Damage);
            if (currentEnemy.hitPoint <= 0 )
            {
                if (!currentEnemy.killedOnce)
                {
                    AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillMagicalAbilities});
                    currentEnemy.killedOnce = true;
                }
               
              
            }
            // Add force to the enemy.
          
        }
    }
    public void SetSkill(float aoe, float dur, float f, float dmg) // Set the private variables of the shockwave.
    {
        Diameter = aoe; Duration = dur; Force = f; Damage = dmg;
    }
}
