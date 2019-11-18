using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    public int unlockedLevels;
    public int resource;
    public int[] orbs;
    public int[] weapons;

    public void SavePlayer()
    {
        DataSystem.SavePlayer(this);
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
