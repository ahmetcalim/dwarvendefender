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
        FindObjectOfType<ResourceHandler>().AddResource(FindObjectOfType<CurrentPlayer>(), mob.cost);
        
    }
}
public enum ResourceTypes
{
    Coins = 0, RuneStones = 1
}