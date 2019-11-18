using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public int[] Resources;

    public bool[] InstancesWon;
    public bool[] InstancesUnlocked;
    public int UnlockedEntries;

    public int[] UpgradeLevels;
    public bool[] WeaponUnlocks = { true, true, false };

    public void GrabAndWrite(string path)
    {
        ResourceManager rm = ResourceManager.resourceManager;
        CampaignManager cm = CampaignManager.campaignManager;




        Resources = rm.ResourceAmounts;

        InstancesWon = cm.InstanceWon;
        InstancesUnlocked = cm.InstanceUnlocked;
        UnlockedEntries = cm.UnlockedEntries;
        

        string json = JsonUtility.ToJson(this, true);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, json);
        Debug.Log("burayada giriyor");
    }
    
    public static SaveData ReadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        else
        {
            string contents = File.ReadAllText(path);
            if (string.IsNullOrEmpty(contents))
            {
                return null;
            }

            return JsonUtility.FromJson<SaveData>(contents);
        }
    }
}
