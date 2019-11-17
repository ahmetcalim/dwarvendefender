using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private static AchievementManager s_Instance = null;
    public List<Achievement> achievements;
    public GameEvents gameEvent;
    public AchievementFeedBack aFeedBack;
    internal bool onLoad;
    static AchievementManager checker;
    public static AchievementManager Instance
    {
        get
        {
            checker = GameObject.FindObjectOfType(typeof(AchievementManager)) as AchievementManager;

            if (checker && checker.onLoad)
                return checker;
            else
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(AchievementManager)) as AchievementManager;
                }

                if (s_Instance == null)
                {
                    GameObject obj = new GameObject("_AchievementManager");
                    s_Instance = obj.AddComponent(typeof(AchievementManager)) as AchievementManager;
                }
                DontDestroyOnLoad(s_Instance.gameObject);
                return s_Instance;
            }
        }
    }

    private AchievementManager()
    {
        //private kalsın
    }

    private void OnEnable()
    {
        if (checker)
            Destroy(gameObject);

        if (AchievementManager.Instance.achievements == null || AchievementManager.Instance.achievements.Where(a => a == null).Any())
        {
            Debug.LogError("Check your achievement List");
            Debug.Break();
        }
        gameEvent = GetComponent<GameEvents>();
        onLoad = true;
    }

    internal void SetFeedBackCompenentToAchievement()
    {
        for (int i = 0; i < achievements.Count; i++)
            achievements[i].feedBack = this.aFeedBack;
    }

    public void CheckAchievements(List<Achievement.AchievementType> achievementTypes)
    {
        List<Achievement> result = achievements.Where(a => !a.unlocked).ToList();
        for (int i = 0; i < result.Count; i++)
        {
            if (achievementTypes.Contains(result[i].achievementType))
            {
                switch (result[i].achievementType)
                {
                    case Achievement.AchievementType.Kill:
                        result[i].CountToUnlock--;
                        gameEvent.kill++;
                        break;
                    case Achievement.AchievementType.KillBluedgeoningWeapon:
                        result[i].CountToUnlock--;;
                        gameEvent.bludgeoningKill++;
                        break;
                    case Achievement.AchievementType.KillSlashingWeapon:
                        result[i].CountToUnlock--;
                        gameEvent.slashingKill++;
                        break;
                    case Achievement.AchievementType.KillThrowingWeapon:
                        result[i].CountToUnlock--;
                        gameEvent.throwingKill++;
                        break;
                    case Achievement.AchievementType.KillMagicalAbilities:
                        result[i].CountToUnlock--;
                        gameEvent.magicalKill++;
                        break;
                    case Achievement.AchievementType.KillSingleInstance50:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.KillSingleInstance100:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.KillSingleInstance250:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.Run200:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.Run500:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.Run1000:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireOver800:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireOver850:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireOver900:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireUnder250:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireUnder300:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.SpireUnder400:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.KnockDown:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.CompleteOnlyBludgeoningWeapons:
                        result[i].CountToUnlock--;
                        break;
                    case Achievement.AchievementType.CompleteOnlySlashingWeapons:
                        result[i].CountToUnlock--;
                        break;
                    default:
                        break;
                }
            }
        }
    }


}
