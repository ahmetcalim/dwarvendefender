using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EclipseSlam : Skill
{
    [Tooltip("Shockwave reference prefab.")]
    public GameObject EclipseSlamShockWave; // Reference prefab.
    private float _cooldown = 0; // Cooldown tracker.
    private bool _activated = false; // Activation tracker, use via ActivateForTime(t).
    private MeshRenderer gemRenderer;
    private float emission;
    private float emissionVal;
    public EclipseSlamVFXActivator vFXActivator;
    public GameObject activeEffect;
    public GameObject cooldownEffect;
    public List<GameObject> gems;
    public GameObject currentGem;
    public float currentCooldown;
    private bool abilityUnlocked;
    Binosh activeBinosh;
    public UnityEvent abilityUsed;
    public IEnumerator ActivateForTime(float t) // Use this function to activate the hit.
    {
        _activated = true;

        if (cooldownEffect)
        {
            cooldownEffect.SetActive(false);
            activeEffect.SetActive(true);

        }
      
        yield return new WaitForSeconds(t);

        if (cooldownEffect)
        {
            cooldownEffect.SetActive(true);
            activeEffect.SetActive(false);

        }
     
        Feedback.FeedbackTxt = "NONE";
        _activated = false;
    }

    public void Activate(float t)
    {
        if (_activated) return;
        
        Debug.Log("ECLIPSE SLAM ACTIVATED");
        if (_cooldown == 0 && Duration > 0) StartCoroutine(ActivateForTime(t));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EffectActivator"))
        {
            if (other.gameObject.GetComponentInParent<Binosh>()) // Tentative.
            {
                
                if (_activated && _cooldown == 0) // Check if the skill is good to go.
                {
                    emission = 0f;
                    // Put down and prepare the shockwave.
                    if (cooldownEffect)
                    {
                        cooldownEffect.SetActive(false);
                        activeEffect.SetActive(false);
                    }

                    abilityUsed.Invoke();
                    GameObject slam = Instantiate(EclipseSlamShockWave);
                    slam.GetComponent<EclipseSlamShockWave>().SetSkill(AoE, Duration, Force, Damage);
                    slam.transform.position = other.gameObject.GetComponentInParent<Binosh>().transform.position;
                    Debug.Log("ECLIPSE SLAM USED");
                    // Set cooldown.
                    _cooldown = BaseCooldown * (1 - CooldownReduction);
                    _activated = false; // FOR TESTING PURPOSES, UNCOMMENT THIS LATER.
                }
            }
        }
       
    }

    private void Start()
    {
        abilityUnlocked = _cooldown > 1f;
    
        foreach (var item in FindObjectsOfType<Binosh>())
        {
            if (item.gameObject.activeSelf)
            {
                activeBinosh = item;
            }
        }
        if (activeBinosh)
        {
            gemRenderer = activeBinosh.gemMesh;
            if (activeBinosh.gemMesh.GetComponent<AbilityCooldownParticle>() == null)
            {
                return;
            }

        }
      
      
        activeEffect = activeBinosh.gemMesh.GetComponent<AbilityCooldownParticle>().particleCompleted.gameObject;
        cooldownEffect = activeBinosh.gemMesh.GetComponent<AbilityCooldownParticle>().particle.gameObject;
        if (!abilityUnlocked)

        {

            cooldownEffect.SetActive(false);
            activeEffect.SetActive(false);
            return;

        }
        if (cooldownEffect)
        {
            cooldownEffect.SetActive(abilityUnlocked);
            activeEffect.SetActive(abilityUnlocked);
        }
      
        gemRenderer.sharedMaterial.SetFloat("_EmissionValue", 1f);
        currentCooldown = BaseCooldown * (1 - CooldownReduction);
        //_activated = true; // FOR TESTING PURPOSES, COMMENT THIS LATER.
    }

    private void Update()
    {

        // Tick down cooldown.
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            emission += Time.deltaTime;
            emissionVal = (currentCooldown - _cooldown) / currentCooldown;
            if (_cooldown > (currentCooldown / 2f))
            {
                if (cooldownEffect)
                {
                    cooldownEffect.SetActive(false);
                    activeEffect.SetActive(false);

                }
             
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", emissionVal);
            }
            if (_cooldown <= .1f)
            {
                gemRenderer.sharedMaterial.SetFloat("_EmissionValue", 2f);
                if (cooldownEffect)
                {
                    cooldownEffect.SetActive(true);
                    activeEffect.SetActive(false);
                }
              
            }


            if (_cooldown <= 0)
            {
                _cooldown = 0;
            }
        }

    }

}
