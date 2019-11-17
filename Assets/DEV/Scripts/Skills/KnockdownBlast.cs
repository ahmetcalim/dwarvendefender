using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownBlast : MonoBehaviour
{

    [Tooltip("Duration multiplier that changes visible duration, compared to active duration.")]
    public float DecayMult;

    [Tooltip("Arbitrary force multiplier.")]
    public float ForceMult;

    [Tooltip("Force gates that will trigger animations. Make sure they are ordered.")]
    public float[] ForceRequirements = { 0f, 1500f }; 

    [Tooltip("Animation trigger names that the gates will trigger. Make sure they are ordered.")]
    public string[] AnimatorTriggers = { "stagger", "getup" };


    private float Force = 4000, Duration = 5f, AoE = .5f, Damage = 1; // Skill variables.
    private float _vel = .1f, _timer = 0, _decayTime = 6.25f; // Local trackers of time and derived variables.
    
    public void SetSkill(float f, float dur, float aoe, float dmg)
    {
        Force = f; Duration = dur; AoE = aoe; Damage = dmg; // Set private variables of the skill.

        _vel = AoE / Duration; // Calculate velocity (units per second)
        _decayTime = Duration * DecayMult; // Calculate decay time (seconds)
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime; // Increment timer.
        var _add = _vel * Time.deltaTime; // Calculate the addition to the size.
        transform.localScale += new Vector3(_add, _add, _add); // Increase size.

        // Fade away.
        Color c = GetComponent<MeshRenderer>().material.color; 
        c.a = 1 - (_timer / _decayTime);
        GetComponent<MeshRenderer>().material.color = c;

        if (_timer > _decayTime) Destroy(gameObject);
    }

    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 0); // Initialize scale at 0.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_timer > Duration) return; // If the skill is inactive, return.
        if (other.GetComponentInParent<Mob>()) // If this is an enemy and it has an animator:
        {
            for (int i = ForceRequirements.Length - 1; i >= 0; i--) // YEAH I'M GOING BACKWARDS WHAT OF IT
            {
                if(Force * ForceMult > ForceRequirements[i]) // If the checked force gate is surpassed:
                {
                    other.GetComponentInParent<Mob>().Stagger(); // Trigger the corresponding animation.
                    break; // Make sure not to trigger anything else, since that may cause issues.
                }
            }
            other.GetComponentInParent<Mob>().TakeDamage(Damage);
        }
    }

}
