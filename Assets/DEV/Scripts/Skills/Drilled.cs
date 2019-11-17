using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drilled : Skill
{
    // Use speed in degrees/second
    // Speed for angular speed base
    // Duration for combo duration base
    // EffectPower for max swords
    // Range for point distance base

    [Tooltip("Prefab reference for the sword.")]
    public GameObject SwordPrefab;
    [Tooltip("How smooth the swords will move. 1 is snappy, 0 is no movement at all.")]
    public float LerpSpeed = 0.5f;
    private List<GameObject> _swords = new List<GameObject>();
    [Tooltip("Reference point that will be manipulated.")]
    public Transform ReferencePoint;

    // Variables to track skill values adjusted for combo count.
    private float _comboTimer;
    private float _spd;
    private float _r;

    // Start is called before the first frame update
    void Start()
    {
        _r = Range; _spd = Speed; _comboTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (EffectPower <= 0) return;
        ReferencePoint.RotateAround(transform.position, transform.up, _spd * Time.deltaTime); // Rotate
        ReferencePoint.transform.position = transform.position + ReferencePoint.forward * _r; // Range adjusting.
        if(_swords.Count > 0)
        {
            float anglePerSword = 360 / _swords.Count;
            for(int i = 0; i < _swords.Count; i++) // For every sword:
            {
                _swords[i].transform.position = Vector3.Lerp(_swords[i].transform.position, ReferencePoint.position, LerpSpeed); // Have the sword approach intended position.
                _swords[i].transform.rotation = Quaternion.Lerp(_swords[i].transform.rotation, ReferencePoint.rotation, LerpSpeed); // Have the sword approach intended rotation.
                ReferencePoint.RotateAround(transform.position, transform.up, anglePerSword); // Have the reference point rotate around the player.
            }
        }

        // Keep track of the combo timer.
        if(_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
            if(_comboTimer <= 0)
            {
                RemoveSword();
            }
        }
    }

    public void AddSword()
    {
        if (_swords.Count >= EffectPower) return; // Too many swords already, return.
        GameObject newSword = Instantiate(SwordPrefab); // Instantiate a sword.
        newSword.transform.position = transform.position; // Position it on the player, it'll look like it's coming out of them.
        _swords.Add(newSword); // Add reference to the list.
        newSword.transform.localScale = new Vector3(newSword.transform.localScale.x, newSword.transform.localScale.y, -newSword.transform.localScale.z);// THIS IS TEMP.
        RecalculateVariables();
    }

    private void RecalculateVariables()
    {

        _comboTimer = Duration - 0.5f * (_swords.Count - 1);
        _spd = Speed + 15 * (_swords.Count - 1);

        _r = Range;
        var upMaxLevel = 5;
        for(int i = 0; i < _swords.Count; i++)
        {
            _r += (upMaxLevel - i) * 0.1f;
        }
    }

    public void RemoveSword()
    {
        if (_swords.Count == 0) return;
        GameObject r = _swords[_swords.Count - 1]; // store reference since we'll delete it from the list.
        _swords.Remove(r); // remove from list
        Destroy(r.gameObject); // use reference to destroy gameObject
        if(_swords.Count > 0) RecalculateVariables();
    }
}
