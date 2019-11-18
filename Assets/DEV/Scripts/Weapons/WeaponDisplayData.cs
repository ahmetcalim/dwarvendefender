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
}
