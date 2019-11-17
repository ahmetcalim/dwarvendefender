using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticismOfMatter : Skill
{
    // From skill we'll use AoE, Damage, Duration.
    // We'll also use MiscParam1, for a chance of explosion (0,1)
    [Tooltip("Prefab of the blast.")]
    public GameObject BlastPrefab;

    [Tooltip("Time in seconds before enemy detonates.")]
    public float DetonationTime = 0.5f;

    private List<Mob> _blastedEnemies = new List<Mob>();
    public IEnumerator DeployBlastInSeconds(Transform target, float t)
    {
        yield return new WaitForSeconds(t);
        DeployBlast(target.position);
    }

    public void DeployBlast(Vector3 deployPos)
    {

        Debug.Log("BOOM");
        var blast = Instantiate(BlastPrefab);
        blast.transform.position = deployPos;
        blast.GetComponent<DamageBlast>().SetSkill(Damage, Duration, AoE);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Duration <= 0) return;
        Mob enemy = other.gameObject.GetComponentInParent<Mob>();
        if(enemy)
        {
            if (enemy.hitPoint > 0)
            {
                return;
            }
            if (_blastedEnemies.Contains(enemy)) return; // already tried this one
            _blastedEnemies.Add(enemy);
            float rng = UnityEngine.Random.Range(0f, 1f);
            Debug.Log("Roll for boom: " + rng.ToString());
            if (rng < 1 - MiscParam1) return; // explosion fails
            StartCoroutine(DeployBlastInSeconds(enemy.transform, DetonationTime));
        }
    }
}
