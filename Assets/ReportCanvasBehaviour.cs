using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportCanvasBehaviour : MonoBehaviour
{
    public Text killCount;
    public Text timePlayed;
    public Text runeStoneEarned;
    public Text coinEarned;
    void OnEnable()
    {
        killCount.text = AchievementManager.Instance.gameEvent.kill.ToString();
        timePlayed.text = Spire.timePlayed.ToString("F0") + " Seconds";
        runeStoneEarned.text = FindObjectOfType<ResourceManager>().AmountsEarned[1].ToString();
        coinEarned.text = FindObjectOfType<ResourceManager>().ResourceAmounts[0].ToString();
        Spire.timePlayed = 0f;
        MobSpawn.killCount = 0;
        
    }
}
