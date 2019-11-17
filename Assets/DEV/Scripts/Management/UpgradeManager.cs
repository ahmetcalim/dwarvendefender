using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager upgradeManager;

    public int WeaponCount = 5;
    public int UpgradesPerWeapon = 6;
    public Weapon[] Weapons;
    public Weapon[] SceneWeapons;
    [Tooltip("All upgrades listed by their weapon and index order. Be careful.")]
    public Upgrade[] Upgrades;
    public bool[] WeaponUnlocks = { true, true, false };

    // Start is called before the first frame update
    void Start()
    {
        if (!upgradeManager)
        {
            upgradeManager = this;
            DontDestroyOnLoad(gameObject);
            SaveData save = SaveData.ReadFromFile("mysave");
            if (save != null)
            {
                int[] UpgradeLevels = save.UpgradeLevels;
                for(int i = 0; i < UpgradeLevels.Length; i++)
                {
                    Upgrades[i].UpgradeLevel = UpgradeLevels[i];
                }
                WeaponUnlocks = save.WeaponUnlocks;
            }
            CheckWeaponUnlocks();
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(AutoStart());
    }
    IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(2f);
      //  FindObjectOfType<CampaignManager>().LoadInstance(1);
    }
    public Upgrade[] GetUpgradesByIndex(int index)
    {
        Upgrade[] ret = new Upgrade[UpgradesPerWeapon];
        for(int i = 0; i < UpgradesPerWeapon; i++)
        {
            ret[i] = Upgrades[UpgradesPerWeapon * index + i];
        }
        return ret;
    }

    public void CheckWeaponUnlocks()
    {
        Debug.Log("Checking weapon unlocks.");
        if (SceneWeapons.Length == 0) return;
        foreach (Weapon w in SceneWeapons)
        {
            if (!w.EnemyWeapon && WeaponUnlocks[w.upgradeIndex])
            {
                w.gameObject.SetActive(true);
                w.WeaponUnlocked = true;
            }
            if(!w.EnemyWeapon && !WeaponUnlocks[w.upgradeIndex])
            {
                w.gameObject.SetActive(false);
                w.WeaponUnlocked = false;
            }
        }
    }
}
