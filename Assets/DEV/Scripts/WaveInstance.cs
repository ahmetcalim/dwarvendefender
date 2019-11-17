using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInstance : MonoBehaviour
{
    public List<Wave> waves;
    private float gameTime;
    private int mobCountSumAll;
    public int GetMobCountSumAll()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            mobCountSumAll += waves[i].nMobSum;
        }
        return mobCountSumAll;
    }
    public float GetGameTimeForInstance()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            gameTime += waves[i].nMobSum * waves[i].spawnTick;
        }
        return gameTime;
    }
}
