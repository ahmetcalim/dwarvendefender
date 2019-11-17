using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string UpgradeName;
    public UpgradeVariables[] AffectedVariables;
    public MagnitudeLine[] UpgradeMagnitudes;
    public int[] UpgradeCosts;
    public ResourceTypes[] UpgradeCostTypes;
    public bool Unlocked;
    public int UpgradeLevel;

    public string UpgradeDescription;
    public Sprite UpgradeIcon;
}

public enum UpgradeVariables
{
    Damage, DamageOverTime, Force, Speed, EffectPower,
    Aoe, Range, Angle, Duration, EffectDuration,
    CooldownReduction,
    Misc1, Misc2, Misc3
}

[System.Serializable]
public class MagnitudeLine
{
    public float[] data;
}

