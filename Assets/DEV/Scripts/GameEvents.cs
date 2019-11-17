using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour
{
    public UnityEvent GameFinished = new UnityEvent();
    public float DistanceTravelled = 0;
    public int kill;
    public int bludgeoningKill;
    public int slashingKill;
    public int magicalKill;
    public int throwingKill;
    static Transform player;
    public Vector3 lastPosition;
    private bool onlyBludgeoning;
    private bool onlySlashing;
    void OnEnable()
    {
        player = Camera.main.transform;
        GameFinished.AddListener(SetGameFinished);
        DistanceTravelled = 0f;
        lastPosition = player.transform.position;
    }
    public void ResetGameEvents()
    {
        OnEnable();
        kill = 0;
        bludgeoningKill = 0;
        slashingKill = 0;
        magicalKill = 0;
        throwingKill = 0;
        run1 = false;
        run2 = false;
        run3 = false;
        onlySlashing = false;
        onlyBludgeoning = false;
        AchievementManager.Instance.aFeedBack = FindObjectOfType<AchievementFeedBack>();
        AchievementManager.Instance.SetFeedBackCompenentToAchievement();
    }
    private static bool run1, run2, run3;
    private static bool kill1, kill2, kill3;
    void Update()
    {

        if (player)
        {
            DistanceTravelled += Vector3.Distance(player.transform.position, lastPosition);
            lastPosition = player.transform.position;
        }
        else
        {
            player = Camera.main.transform;
        }
        

        if (bludgeoningKill > 0 && slashingKill == 0)
        {
            if (!onlyBludgeoning)
            {
                AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.CompleteOnlyBludgeoningWeapons });
                onlyBludgeoning = true;
            }

        }
        if (slashingKill > 0 && bludgeoningKill == 0)
        {
            if (!onlySlashing)
            {
                AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.CompleteOnlySlashingWeapons });
                onlySlashing = true;
            }

        }
        //Run
        if (DistanceTravelled >= 200 && !run1)
        {
            run1 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Run200 });
        }
        if (DistanceTravelled >= 500 && !run2)
        {
            run2 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Run500 });
        }
        if (DistanceTravelled >= 1000 && !run3)
        {
            run3 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.Run1000 });
        }


        if (kill >= 50 && !kill1)
        {
            kill1 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.KillSingleInstance50 });
        }
        if (kill >= 100 && !kill2)
        {
            kill2 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.KillSingleInstance100 });
        }
        if (kill >= 250 && !kill3)
        {
            kill3 = true;
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.KillSingleInstance250 });
        }
    }

    void SetGameFinished()
    {
        //Spire Over
        if (Spire.GetSpireCurrentHealth() >= 800 && Spire.GetSpireCurrentHealth() < 850)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireOver800 });
        }
        if (Spire.GetSpireCurrentHealth() >= 850 && Spire.GetSpireCurrentHealth() < 900)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireOver850 });
        }
        if (Spire.GetSpireCurrentHealth() >= 900)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireOver900 });
        }

        //Spire Under
        if (Spire.GetSpireCurrentHealth() <= 250)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireUnder250 });
        }
        if (Spire.GetSpireCurrentHealth() > 250 && Spire.GetSpireCurrentHealth() <= 300)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireUnder300 });
        }
        if (Spire.GetSpireCurrentHealth() > 300 && Spire.GetSpireCurrentHealth() <= 400)
        {
            AchievementManager.Instance.CheckAchievements(new List<Achievement.AchievementType> { Achievement.AchievementType.SpireUnder400 });
        }


    }

}
