using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetributionWave : MonoBehaviour
{

    [Tooltip("Arbitrary decay control variable. Multiplies with time.")]
    public float DecayMult;
    

    private float _decayTime; // Fadeout time.
    private float _effectiveTime; // Effective time.
    private float _time; // Keeps track of active time.
    private float Velocity;
    private float Range, Size; // Maximum range and scale multiplier.
    private float Damage; 

    void Start()
    {
        _effectiveTime = 1; _decayTime = 5;
        Velocity = 50;
    }

    void Update()
    {
        _time += Time.deltaTime; // Add to active time.

        // transform.position += transform.forward * Velocity * Time.deltaTime; // Move forward.
        GetComponent<Rigidbody>().velocity = transform.forward * 10f;
        if (_time > _decayTime) Destroy(gameObject); // Check active duration.
        if (_time > _effectiveTime) GetComponent<Collider>().enabled = false; // Check collidable duration.
        
    }

    public void SetSkill(float r, float size, float dmg, float dur) // Set the private variables of the shockwave.
    {
        Range = r; Size = size; Damage = dmg;

        // Set relevant variables of the skill.
        //transform.localScale = new Vector3(transform.localScale.x * Size, transform.localScale.y, transform.localScale.z * Size);
        _effectiveTime = dur;
        Velocity = Range / dur;
        _decayTime = _effectiveTime * DecayMult;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (other.GetComponentInParent<MobParent>())
            {
                if (other.GetComponentInParent<MobParent>().mob)
                {
                    if (other.GetComponentInParent<MobParent>().mob.hitPoint <= 0)
                    {
                        if (!other.GetComponentInParent<MobParent>().mob.killedOnce)
                        {
                            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Kill, Achievement.AchievementType.KillMagicalAbilities });
                            other.GetComponentInParent<MobParent>().mob.killedOnce = true;
                        }
                    }
                }
            }
        
        }
     
       
    }
}
