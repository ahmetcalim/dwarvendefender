using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float Damage, DamageOverTime, Force, Speed, EffectPower;
    public float AoE, Range, Angle, Duration, EffectDuration;
    public float CooldownReduction, BaseCooldown;
    public float MiscParam1, MiscParam2, MiscParam3;
    
    private float _aojLast = 1.0f;

    public string[] TriggerNames;

    public void ApplyUpgrade(Upgrade u)
    {
        if (!u) return; // i meannnnnnnnnnn
        if (u.AffectedVariables.Length == 0) return; // if the upgrade does nothing, return.
        if (u.UpgradeLevel == 0) return; // if the upgrade, in fact, didn't upgrade, return.
        if (u.UpgradeLevel > u.UpgradeCosts.Length) return; // if level is too high, return.
        foreach (string s in TriggerNames)
        {
            if (GameObject.Find(s)) GameObject.Find(s).SetActive(true);
        }

        for (int i = 0; i < u.AffectedVariables.Length; i++)
        {
            for(int j = 0; j < u.UpgradeLevel; j++)
            {
                if (u.AffectedVariables[i] == UpgradeVariables.Damage)
                {
                    Damage += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.DamageOverTime)
                {
                    DamageOverTime += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Force)
                {
                    Force += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Speed)
                {
                    Speed += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.EffectPower)
                {
                    EffectPower += u.UpgradeMagnitudes[i].data[j];
                }

                else if (u.AffectedVariables[i] == UpgradeVariables.Aoe)
                {
                    AoE += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Range)
                {
                    Range += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Angle)
                {
                    Angle += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Duration)
                {
                    Duration += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.EffectDuration)
                {
                    EffectDuration += u.UpgradeMagnitudes[i].data[j];
                }

                else if (u.AffectedVariables[i] == UpgradeVariables.CooldownReduction)
                {
                    CooldownReduction += u.UpgradeMagnitudes[i].data[j];
                }

                else if (u.AffectedVariables[i] == UpgradeVariables.Misc1)
                {
                    MiscParam1 += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Misc2)
                {
                    MiscParam2 += u.UpgradeMagnitudes[i].data[j];
                }
                else if (u.AffectedVariables[i] == UpgradeVariables.Misc3)
                {
                    MiscParam3 += u.UpgradeMagnitudes[i].data[j];
                }
            } 
        }
    }

   
}
