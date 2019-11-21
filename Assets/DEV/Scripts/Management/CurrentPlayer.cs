using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    public int unlockedLevels;
    public int resource;
    public int[] orbs;
    public int[] weapons;

    private void Awake()
    {
        if (DataSystem.PlayerExist())
        {
            LoadPlayer();
        }
    }
    public void SavePlayer()
    {
        DataSystem.SavePlayer(this);
    }
    public void ResetProgress()
    {
        unlockedLevels = 1;
        resource = 0;
        orbs = new int[8];
        weapons = new int[8];
        for (int i = 0; i < orbs.Length; i++)
        {
            orbs[i] = 0;
            weapons[i] = 0;
        }
        weapons[0] = 1;
        weapons[1] = 1;
        SavePlayer();
    }
    public void LoadPlayer()
    {
        PlayerData data = DataSystem.LoadPlayer();
        unlockedLevels = data.unlockedLevels;
        resource = data.resource;
        orbs = data.orbs;
        weapons = data.weapons;
    }
}
