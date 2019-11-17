using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int id;
    public int nMobSum;
    public int nSpawnSize;
    public float spawnTick;
    public Wave nextWave;
    public MobPrefabList mobPrefabLists = new MobPrefabList();
}

[System.Serializable]
public class MobPrefab
{
    public List<Mob> mobPrefabs;
    public List<int> prefabMobCount;
}
[System.Serializable]
public class MobPrefabList
{
    public List<MobPrefab> mobTypes;
}