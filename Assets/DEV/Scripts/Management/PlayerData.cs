using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int unlockedLevels;
    public int resource;
    public int[] orbs;
    public int[] weapons;
    public PlayerData(CurrentPlayer player)
    {
        unlockedLevels = player.unlockedLevels;
        resource = player.resource;
        orbs = player.orbs;
        weapons = player.weapons;
    }
}
