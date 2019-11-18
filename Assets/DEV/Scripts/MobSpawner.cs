using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MobSpawn
{
    private GameObject mobCopy;
    private int mobTypeCount;
    private int mobPrefabCount;
    private Wave currentWaveForPooledMobs;
    public static int mobCountSumCurrent;
    private void InstantiateAllMobsForThisInstance()
    {
        for (int mobTypeC = 0; mobTypeC < currentWaveForPooledMobs.mobPrefabLists.mobTypes.Count; mobTypeC++)
        {
            for (int mobTypePrefabC = 0; mobTypePrefabC < currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].mobPrefabs.Count; mobTypePrefabC++)
            {
                for (int b = 0; b < currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].prefabMobCount[mobTypePrefabC]; b++)
                {
                    GameObject mobCopy = Instantiate(currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].mobPrefabs[mobTypePrefabC].gameObject, transform);
                    mobCopy.transform.localPosition = Vector3.zero;
                }
            }
        }

    }
}
