using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementTrigger : Skill
{
    // miscparam1 is spawn frequency

    private float _cooldown = 0; // Cooldown tracker.
    private bool _activated = false;
    private int _pieceCount = 0;
    public GameObject MeteorSpawnerPrefab;

    public float Height = 200;

    public float ActivationTime = 2.0f;
    private float _timeToActivation = 2.0f;

    private void Activate()
    {
        if (Duration <= 0) return;
        _activated = false;
        _timeToActivation = ActivationTime;
        if (_cooldown > 0) return;

        GameObject ms = Instantiate(MeteorSpawnerPrefab);
        ms.transform.position = transform.position;
        ms.transform.position += transform.up * Height;

        ms.GetComponent<JudgementSpawner>().SetSkill(Damage, Speed, AoE, Duration, Range, MiscParam1);

        // Set cooldown.
        _cooldown = BaseCooldown * (1 - CooldownReduction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sword>() || other.gameObject.GetComponentInParent<Sword>())
        {
            _pieceCount++;
            if (!_activated && _cooldown == 0)
            {
                _activated = true;
                _timeToActivation = ActivationTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Sword>() || other.gameObject.GetComponentInParent<Sword>())
        {
            _pieceCount--;
            if (_activated && _pieceCount <= 0)
            {
                _activated = false;
                _timeToActivation = ActivationTime;
            }
        }
    }

    private void Update()
    {
        // Tick down cooldown.
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0) _cooldown = 0;
        }
        // Tick up time to activation.
        else if (_activated)
        {
            _timeToActivation -= Time.deltaTime;
            if (_timeToActivation <= 0) Activate();
        }
    }
}
