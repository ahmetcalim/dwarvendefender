using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Achievement : ScriptableObject
{
    public string achievementName;
    public string achievementDescription;
    public AchievementType achievementType;
    public RewardType reward;
    public int rewardAmount;
    [SerializeField]
    internal int countToUnlock;
    UnityEvent Success = new UnityEvent();
    internal AchievementFeedBack feedBack;
    public Achievement()
    {
        Success.AddListener(HandlerSuccessEvent);
    }

    public int CountToUnlock
    {
        get { return countToUnlock; }
        set
        {
            countToUnlock = value;

            if (countToUnlock <= 0)
            {
                if (!unlocked)
                    Success.Invoke();
                unlocked = true;
            }
        }
    }
    public bool unlocked;
    public Sprite icon, feedBackIcon;
    void HandlerSuccessEvent()
    {
        Debug.Log("Success: " + achievementName);
        if (feedBack)
        {
            feedBack.SetFeedBack(this);
        }
        switch (reward)
        {
            case RewardType.Gold:
                ResourceManager.resourceManager.ResourceAmounts[0] += rewardAmount;
                break;
            case RewardType.RuneStone:
                ResourceManager.resourceManager.ResourceAmounts[0] += rewardAmount;
                break;
            case RewardType.DamageAllWeapons:
                break;
            case RewardType.DamageSlashingWeapons:
                break;
            case RewardType.DamageBludgeoningWeapons:
                break;
            case RewardType.DamageAllRangedAttacks:
                break;
            case RewardType.CooldownReduce:
                break;
            default:
                break;
        }


    }
    public enum AchievementType { Kill, KillBluedgeoningWeapon, KillSlashingWeapon, KillThrowingWeapon, KillMagicalAbilities, KillSingleInstance50, KillSingleInstance100, KillSingleInstance250, Run200, Run500, Run1000, SpireOver800, SpireOver850, SpireOver900, SpireUnder250, SpireUnder300, SpireUnder400, KnockDown, CompleteOnlyBludgeoningWeapons, CompleteOnlySlashingWeapons }
    public enum RewardType { Gold, RuneStone, DamageAllWeapons, DamageSlashingWeapons, DamageBludgeoningWeapons, DamageAllRangedAttacks, CooldownReduce }


}
