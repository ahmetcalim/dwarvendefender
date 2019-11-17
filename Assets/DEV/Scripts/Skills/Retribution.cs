using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Retribution : Skill
{
    [Tooltip("Wave reference prefab.")]
    public GameObject RetributionWave;

    [Tooltip("Reference point to keep track of.")]
    public Transform ReferencePoint;

    [Tooltip("Speed required to start the cut.")]
    public float SwingSpeed;

    private Vector3 _posLast;
    private Quaternion _rotLast;
    private float _cooldown;
    private bool _activated = false, _cutting;
    private Vector3 _cutStartPos, _cutEndPos;
    private Quaternion _cutStartRot, _cutEndRot;
    public float currentCooldown;
    public MeshRenderer gemRenderer;
    private float emission;
    private float emissionVal;
    public TextMeshPro cooldownTxt;
    public TextMeshPro cooldown;
    public bool abilityUnlocked;
    public GameObject activeEffect;
    public GameObject cooldownEffect;

    public IEnumerator ActivateForTime(float t) // Use this function to activate the hit.
    {
        _activated = true;
        activeEffect.SetActive(true);
        cooldownEffect.SetActive(false);
        StartCoroutine(GetComponentInParent<VRControllerHandler>().ExecuteHaptic());
        yield return new WaitForSeconds(t);
        activeEffect.SetActive(false);
        cooldownEffect.SetActive(true);
        _activated = false;
    }
    
    public void Activate(float t)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 0) return;

        if (_activated) return;
        if (_cooldown == 0 && Duration > 0) StartCoroutine(ActivateForTime(t));
    }

    // Start is called before the first frame update
    void Start()
    {
        abilityUnlocked = _cooldown > 1;
        if (gemRenderer.GetComponent<AbilityCooldownParticle>().particleCompleted == null)
        {
            return;
        }
        activeEffect = gemRenderer.GetComponent<AbilityCooldownParticle>().particleCompleted.gameObject;
        cooldownEffect = gemRenderer.GetComponent<AbilityCooldownParticle>().particle.gameObject;
        if (!abilityUnlocked)
        {
            activeEffect.SetActive(false);
            cooldownEffect.SetActive(false);
            return; }
        
        _posLast = ReferencePoint.position;
        _rotLast = ReferencePoint.rotation;
        currentCooldown = BaseCooldown * (1 - CooldownReduction);
        // _activated = true; // FOR TESTING PURPOSES, COMMENT THIS LATER.
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTxt.text = _cooldown.ToString("F1");
        cooldown.text = emissionVal.ToString("F1");
        if (GetComponent<Rigidbody>().velocity.magnitude > 0) return;
        Vector3 refVel = (ReferencePoint.position - _posLast) / Time.deltaTime;

        // Tick down cooldown.
        if (_cooldown > 0)
        {
            activeEffect.SetActive(false);
            cooldownEffect.SetActive(false);
            _cooldown -= Time.deltaTime;
            emissionVal = (currentCooldown - _cooldown) / currentCooldown;
            if (_cooldown > (currentCooldown / 2f))
            {
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", emissionVal);
            }
            if(_cooldown <= .1f)
            {
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", 2f);
                activeEffect.SetActive(false);
                cooldownEffect.SetActive(true);
            }
            
            if (_cooldown <= 0) _cooldown = 0;
        }

        // Start the cut.
        if (!_cutting)
        {
            if (refVel.magnitude > SwingSpeed && _activated && _cooldown == 0)
            {
                _cutting = true;
                _cutStartPos = ReferencePoint.position;
                _cutStartRot = ReferencePoint.rotation;
            }
        }

        // End the cut.
        if (_cutting)
        {
            if (GetComponent<Weapon>().transform.parent != null)
            {
                
                if (Vector3.Distance(ReferencePoint.position, _cutStartPos) > RetributionWave.transform.localScale.x) // The cut succeeds.
                {

                    _cooldown = BaseCooldown * (1 - CooldownReduction); // Set cooldown
                    _activated = false; // FOR TESTING PURPOSES, UNCOMMENT THIS LATER.
                    _cutEndPos = ReferencePoint.position;
                    _cutEndRot = ReferencePoint.rotation;
                    CreateWave();
                    emissionVal = 0f;
                    _cutting = false;
                }
            }
        }

        _posLast = ReferencePoint.position;
        _rotLast = ReferencePoint.rotation;
    }

    void CreateWave()
    {
        activeEffect.SetActive(false);
        cooldownEffect.SetActive(false);
        GameObject wave = Instantiate(RetributionWave); // Instantiate wave.
        wave.transform.position = (_cutStartPos + _cutEndPos) / 2; // Set wave transform.
        wave.transform.rotation = _cutEndRot;
        wave.GetComponent<RetributionWave>().SetSkill(Range, AoE, Damage, Duration); // Prepare wave statistics.
        if (wave.GetComponent<ResentmentWave>()) wave.GetComponent<ResentmentWave>().SetSkill(DamageOverTime, EffectDuration);
    }
}
