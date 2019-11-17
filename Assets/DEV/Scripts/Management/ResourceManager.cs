using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager resourceManager;

    private int _typeCount = System.Enum.GetValues(typeof(ResourceTypes)).Length;
    public int[] ResourceAmounts; // Currently available resources.
    public string[] ResourceNames; // Names of the resources.
    public Sprite[] ResourceSprites; // Sprite represantations of resources.

    public int[] AmountsEarned = new int[2]; // Amounts earned from the current instance.
    public Text[] EarningDisplays; // Where to display the earnings.

    public static bool isFirstWave;

    public int mobOnWave;
    public int killedMobOnWave;
    public static int mobCount;
    // Start is called before the first frame update
    void Awake()
    {
       
        // Highlander.
        DontDestroyOnLoad(this);
        if (!resourceManager)
        {
            resourceManager = this;
            if (SaveData.ReadFromFile("mysave") != null)
            {
            
                ResourceAmounts = SaveData.ReadFromFile("mysave").Resources;
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKill(Mob mob)
    {
        if (ComboTracker.comboTracker) ComboTracker.comboTracker.IncrementCombo(mob);
        else
        {
            Debug.Log("COMBOTRACKERYOK");
        }
        MobSpawn.killCount++;
        // Debug.Log(MobSpawn.killCount + ". Goblin Öldürüldü");
        AmountsEarned[0] += 1;
        
    }

 
    public void RegisterEarned() // Will be called once player confirms earnings.
    {
        for(int i = 0; i < AmountsEarned.Length; i++)
        {
            ResourceAmounts[i] += AmountsEarned[i]; // Add amounts earned to available resources.
            AmountsEarned[i] = 0; // Clear out for next instance.
        }
    }
    public void GenerateEarningReport() // Will be called once the player finishes the instance.
    {
        string s;
        for (int i = 0; i < AmountsEarned.Length; i++)
        {
            // ResourceName: 1000 + 150 = 1150
            //s = ResourceNames[i] + ": " + ResourceAmounts[i].ToString() + " + " + AmountsEarned[i].ToString() + " = " + (ResourceAmounts[i] + AmountsEarned[i]).ToString(); // Setting up the format.
            s = AmountsEarned[i].ToString();
            EarningDisplays[i].text = s; // Displaying.
        }
    }
}
public enum ResourceTypes
{
    Coins = 0, RuneStones = 1
}