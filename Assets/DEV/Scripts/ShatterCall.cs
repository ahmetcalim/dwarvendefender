using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCall : Skill
{
    
    public GameObject ShatterCallFissure;
    private float _cooldown = 0; // Cooldown tracker.
    private bool _activated = false; // Activation tracker, use via Activate(t).

    public MeshRenderer gemRenderer;
    private float emission;
    private float emissionVal;
    public GameObject activeEffect;
    public GameObject cooldownEffect;
    private bool abilityUnlocked;
    Xopic activeXopic;
    public IEnumerator ActivateForTime(float t) // Use this function to activate the hit.
    {
        _activated = true;
        cooldownEffect.SetActive(false);
        activeEffect.SetActive(true);
        yield return new WaitForSeconds(t);
        cooldownEffect.SetActive(true);
        activeEffect.SetActive(false);
        _activated = false;
    }

    public void Activate(float t)
    {
        if (_activated) return;
        if (_cooldown == 0) StartCoroutine(ActivateForTime(t));
    }
    private void Start()
    {
        foreach (var item in FindObjectsOfType<Xopic>())
        {
            if (item.gameObject.activeSelf)
            {
                activeXopic = item;
            }
        }
        if (!activeXopic)
        {
            return;
        }

        if (activeXopic.gemMesh.GetComponent<AbilityCooldownParticle>().particleCompleted == null)
        {
            return;
        }
        activeEffect = activeXopic.gemMesh.GetComponent<AbilityCooldownParticle>().particleCompleted.gameObject;
        cooldownEffect = activeXopic.gemMesh.GetComponent<AbilityCooldownParticle>().particle.gameObject;
        abilityUnlocked = _cooldown > 1f;
        if (!abilityUnlocked)
        {
            cooldownEffect.SetActive(false);
            activeEffect.SetActive(false);
            return;
        }
        activeEffect.SetActive(abilityUnlocked);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EffectActivator"))
        {
            if (other.GetComponentInParent<Xopic>())
            {
                gemRenderer = other.GetComponentInParent<Xopic>().gemMesh;
                if (_activated && _cooldown == 0) // Check if the skill is good to go.
                {

                    // Put down and prepare the fissure.
                    GameObject fissure = Instantiate(ShatterCallFissure);
                   
                    emission = 0f;
                    Vector3 hammerForwardProjectedNormal = new Vector3(other.transform.forward.x, 0f, other.transform.forward.z).normalized * -1f;

                    fissure.GetComponent<ShatterCallFissure>().SetSkill(AoE, Duration, EffectDuration, Angle, Force, Damage);
                    fissure.transform.position = transform.position + hammerForwardProjectedNormal;
                    fissure.transform.forward = hammerForwardProjectedNormal;
                    cooldownEffect.SetActive(false);
                    activeEffect.SetActive(false);
                    // Set cooldown.
                    _cooldown = BaseCooldown * (1 - CooldownReduction);
                    _activated = false; // FOR TESTING PURPOSES, UNCOMMENT THIS LATER.
                }
            }
        }
    }

    private void Update()
    {
        // Tick down cooldown.
        if (_cooldown > 0)
        {
            emission += Time.deltaTime;
            emissionVal = (emission / BaseCooldown);
            if (emissionVal < .5f)
            {
                cooldownEffect.SetActive(false);
                activeEffect.SetActive(false);
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", emissionVal);
            }
            else
            {
                cooldownEffect.SetActive(true);
                activeEffect.SetActive(false);
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", 1f);
            }
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0) _cooldown = 0;
        }
    }
}
