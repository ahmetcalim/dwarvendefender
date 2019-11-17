using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpawner : MonoBehaviour
{
    public Mob archerPrefab;
    public Transform parent;
    public float spawnTick;
    public MobSpawn mobSpawner;
    public SpawnRules spawnRules;
    public bool available;
    private void Start()
    {
        StartCoroutine(CheckArcherSpawn());
        
    }
    private IEnumerator CheckArcherSpawn()
    {
        yield return new WaitForSeconds(spawnTick);
        if (mobSpawner.MobOnSceneAvailable() && available)
        {
            Spawn();
        }
        StartCoroutine(CheckArcherSpawn());
    }
    public void Spawn()
    {
        GameObject archerCopy = Instantiate(archerPrefab, transform.position, Quaternion.identity).gameObject;
        archerCopy.transform.SetParent(parent);
        archerCopy.GetComponent<Archer_AI>().spawner = this;
        available = false;
    }
}
