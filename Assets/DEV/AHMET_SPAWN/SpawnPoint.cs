using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public List<SpawnPoints> spawnPoints;
    int choosen;
    public Transform GetRandomPoint()
    {
        int probability = Random.Range(0,100);
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (probability >= spawnPoints[i].minimumProbabilityRange && probability <= spawnPoints[i].maximumProbabilityRange)
            {
                choosen = i;
            }
        }
        return spawnPoints[choosen].choosenSpawnPoint;

    }
    
}
[System.Serializable]
public class SpawnPoints
{
    public Transform choosenSpawnPoint;
    public int minimumProbabilityRange = 0;
    public int maximumProbabilityRange = 0;
}

