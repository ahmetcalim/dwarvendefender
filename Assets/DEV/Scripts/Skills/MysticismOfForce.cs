using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticismOfForce : Skill
{
    // From skill we'll use CD, CDR, AoE, Force, Duration.
    [Tooltip("Makes the skill pop every time it's off cooldown. Check true if script is on spire.")]
    public bool IsOnSpire;
    public float SpireBlastSize = 10f;

    [Tooltip("Prefab of the blast.")]
    public GameObject BlastPrefab;

    private float _cooldown; // Keeps track of cooldown.

    // Start is called before the first frame update
    void Start()
    {
        _cooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0) _cooldown = 0;
        }
        if(_cooldown == 0 && IsOnSpire && Duration > 0)
        {
            DeployBlast(transform.position);
        }
    }

    public void DeployBlast(Vector3 deployPos)
    {

        var blast = Instantiate(BlastPrefab);
        blast.transform.position = deployPos;
        if (!IsOnSpire)
            blast.GetComponent<KnockdownBlast>().SetSkill(Force, Duration, AoE, Damage);
        else
            blast.GetComponent<KnockdownBlast>().SetSkill(Force, Duration, SpireBlastSize, Damage);
        _cooldown = BaseCooldown * (1 - CooldownReduction);
    }
}
