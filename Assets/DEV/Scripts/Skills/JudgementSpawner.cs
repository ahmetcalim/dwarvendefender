using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementSpawner : MonoBehaviour
{
    /*
     *  Damage, Speed, AoE (for dropped objects)
     *  Duration, Range, MiscParam1(Spawn delay), Angle (for spawner)
     */

    private float Damage = 1, Speed = 60, AoE = 1;
    private float Duration = 10, Range = 10, SpawnDelay = 0.5f, Angle = 60;

    [Tooltip("Prefab of meteor to be deployed.")]
    public GameObject MeteorPrefab;
    private float _time = 0, _spawnTime = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (_time < Duration)
        {
            _time += Time.deltaTime;
            _spawnTime += Time.deltaTime;
            while (_spawnTime >= SpawnDelay)
            {
                float dist = Random.Range(0, Range);
                float a = Random.Range(0, 5) * Angle;
                DeployMeteor(dist, a);
                _spawnTime -= SpawnDelay;
            }
        }
        else Destroy(gameObject);
    }

    private void DeployMeteor(float dist, float a)
    {
        GameObject m = Instantiate(MeteorPrefab);
        m.transform.position = transform.position;
        m.transform.position += transform.forward * dist;
        m.transform.RotateAround(transform.position, transform.up, a);

        // This is a placeholder. Put SetSkill here once that's there.
        m.GetComponent<Rigidbody>().velocity = new Vector3(0, -Speed, 0);
        m.transform.localScale = new Vector3(AoE, AoE, AoE);
        float destroyTime = (transform.position.y / Speed) + 1;
        Destroy(m, destroyTime);
    }

    public void SetSkill(float dmg, float spd, float aoe, float dur, float r, float spawnT)
    {
        Damage = dmg; Speed = spd; AoE = aoe; Duration = dur; Range = r; SpawnDelay = spawnT;
    }
}
