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
    private void Awake()
    {
     
        
        
    }

    private void InstantiateAllMobsForThisInstance()
    {
        //Kaç wave varsa
        for (int mobTypeC = 0; mobTypeC < currentWaveForPooledMobs.mobPrefabLists.mobTypes.Count; mobTypeC++)
        {
            //Kaç mob type varsa
            for (int mobTypePrefabC = 0; mobTypePrefabC < currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].mobPrefabs.Count; mobTypePrefabC++)
            {
                //Type daki prefab sayısı
                for (int b = 0; b < currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].prefabMobCount[mobTypePrefabC]; b++)
                {
                    //O prefabtan kaç tane olacağı
                    GameObject mobCopy = Instantiate(currentWaveForPooledMobs.mobPrefabLists.mobTypes[mobTypeC].mobPrefabs[mobTypePrefabC].gameObject, transform);
                    mobCopy.transform.localPosition = Vector3.zero;
                   // pooledObjects.Add(mobCopy);
                   // mobCopy.SetActive(false);
                }
            }
        }

    }
}

[System.Serializable]
public class StateList
{
    public AIState[] States;
    public float[] Weights;
};