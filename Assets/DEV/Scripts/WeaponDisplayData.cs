using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDisplayData : MonoBehaviour
{
    public bool WeaponUnlocked;
    public Texture WeaponSprite;
    public string WeaponName;
    public string WeaponDescription;
    public float WeaponDamage;
    public float WeaponReach;
    public string[] UpgradeDescriptions = new string[6];
    public Sprite[] UpgradeIcons = new Sprite[6];
    public Upgrade[] Upgrades;
}
