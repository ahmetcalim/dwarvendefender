using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbassadorOfJustice : Skill
{

    // Put this on the dwarf, pass the combo counter to UpdateSize and watch them grow.
    // Use EffectPower for multiplier. will be .1, .25, .35, .45, .5
    // Use MiscParam1 for gold gain bonus.

    // Weapon data for convenience.
    private Weapon[] weps;
    private Vector3[] _weaponSizes;
    private Vector3[] _weaponTargets;

    public Transform CameraReference;
    public Transform RigReference;
    private Vector3 _defaultRigSize;
    private Vector3 _targetRigSize;
    private Vector3 _defaultCamSize;
    private Vector3 _targetCamSize;

    [HideInInspector] public float SizeMultiplier = 1;

    private bool _growing = false; // See if things need to grow.

    [Tooltip("Arbitrary linear growth speed. Default: 1.0")]
    public float GrowthSpeed = 1.0f;
    [Tooltip("The maximum distance the growth can snap to. Default: 0.1")]
    public float GrowthSnapEpsilon = 0.1f;

    void Start()
    {
        // Initialize default sizes for target objects.
        _defaultRigSize = RigReference.localScale;

        _defaultCamSize = GetGlobalScale(CameraReference);

        weps = FindObjectsOfType<Weapon>(); // Find weapons.
        // Initialize default sizes for weapons.
        _weaponSizes = new Vector3[weps.Length];
        _weaponTargets = new Vector3[weps.Length];
        for(int i = 0; i < weps.Length; i++)
        {
            _weaponSizes[i] = GetGlobalScale(weps[i].transform);
            _weaponTargets[i] = _weaponSizes[i];
        }
    }

    private void Update()
    {
        if (_growing)
        {
            bool canStop = true; // A flag to designate when the growing can stop completely.
            Vector3 oldCamPoint = CameraReference.position;
            RigReference.localScale = Vector3.MoveTowards(RigReference.localScale, _targetRigSize, GrowthSpeed * Time.deltaTime);
            Vector3 moveDirection = oldCamPoint - CameraReference.position;
            RigReference.position += new Vector3(moveDirection.x, 0, moveDirection.z);

            if (Vector3.Distance(RigReference.localScale, _targetRigSize) > GrowthSnapEpsilon
                || Vector3.Distance(CameraReference.localScale, _targetCamSize) > GrowthSnapEpsilon) canStop = false;

            if (canStop) // If the flag didn't fall:
            {
                _growing = false; // The growth can stop.
                oldCamPoint = CameraReference.position;
                RigReference.localScale = _targetRigSize;
                moveDirection = oldCamPoint - CameraReference.position;
                RigReference.position += new Vector3(moveDirection.x, 0, moveDirection.z);
            }
        }
        for (int i = 0; i < weps.Length; i++) // For each weapon:
        {
            if (weps[i].transform.GetComponentInParent<AmbassadorOfJustice>() == null) // If this is not the parent:
            {
                // Unparent weapon, scale it, reparent it.
                Transform wep = weps[i].transform;
                MoveTowardsGlobalScale(wep, _weaponTargets[i]);
            }
        }
    }

    public void UpdateSize(int combo) // Call this on combo counter change.
    {
        // For each weapon and target object, set target size.
        SizeMultiplier = (1 + combo * EffectPower);
        _targetRigSize = _defaultRigSize * SizeMultiplier;
        _targetCamSize = _defaultCamSize * SizeMultiplier;
        for(int i = 0; i < weps.Length; i++)
        {
            _weaponTargets[i] = _weaponSizes[i] * SizeMultiplier;
        }
        _growing = true; // Raise the growth flag.
    }

    public int CalculateGains() {
        int winCount = 0;
        foreach(bool w in CampaignManager.campaignManager.InstanceWon)
        {
            if (w)
            {
                winCount++;
            }
        }
        return winCount * Mathf.FloorToInt(MiscParam1);
    }

    private Vector3 GetGlobalScale(Transform t)
    {
        Transform p = t.parent;
        t.SetParent(null);
        Vector3 ret = t.localScale;
        t.SetParent(p);
        return ret;
    }
    private void MoveTowardsGlobalScale(Transform t, Vector3 s)
    {
        Transform p = t.parent;
        t.SetParent(null);
        t.localScale = Vector3.MoveTowards(t.localScale, s, GrowthSpeed * Time.deltaTime);
        t.SetParent(p);
    }
    private void SetGlobalScale(Transform t, Vector3 s)
    {
        Transform p = t.parent;
        t.SetParent(null);
        t.localScale = s;
        t.SetParent(p);
    }
}
