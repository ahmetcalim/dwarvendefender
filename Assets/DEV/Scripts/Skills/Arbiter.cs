using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbiter : Skill
{
    [Tooltip("Shockwave reference prefab.")]
    public GameObject ArbiterShockWave; // Reference prefab.
    private float _cooldown = 0; // Cooldown tracker.
    private bool _activated = false; // Activation tracker, use via ActivateForTime(t).


    public IEnumerator ActivateForTime(float t)
    {
        _activated = true;
        yield return new WaitForSeconds(t);
        _activated = false;
    }

    public void Activate(float t) // This will be called from the double-tap ground trigger.
    {
        if (!gameObject.activeSelf) return;
        if (_cooldown == 0 && Duration > 0) StartCoroutine(ActivateForTime(t));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInParent<Mob>()) // Check if we slapped an enemy.
        {
            if (_activated && _cooldown == 0) // Check if the skill is good to go.
            {
                // Put down and prepare the shockwave.

                GameObject shockwave = Instantiate(ArbiterShockWave);
                shockwave.GetComponent<KnockdownBlast>().SetSkill(Force, Duration, AoE, 0);
                shockwave.transform.position = transform.position;

                // Set cooldown.
                _cooldown = BaseCooldown * (1 - CooldownReduction);
                _activated = false; // FOR TESTING PURPOSES, UNCOMMENT THIS LATER.
            }
        }
    }

    private void Update()
    {
        if(_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0) _cooldown = 0;
        }
    }
}
