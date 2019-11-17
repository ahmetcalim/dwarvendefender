using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementActivator : MonoBehaviour
{
    public Achievement achievement;
    public Image achievementImage;

    private void OnEnable()
    {
        if (achievement.unlocked)
        {
            achievementImage.sprite = achievement.icon;
        }
    }
}
