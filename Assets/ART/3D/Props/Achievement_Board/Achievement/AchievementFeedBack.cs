using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementFeedBack : MonoBehaviour
{
    public Image feedBackImage;
    public GameObject obj;
    public void SetFeedBack(Achievement achievment)
    {
        feedBackImage.sprite = achievment.feedBackIcon;
        StartCoroutine(Set());
    }

    IEnumerator Set()
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(4.3f);
        obj.SetActive(false);
    }
}
