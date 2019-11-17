using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossDwarf : MonoBehaviour
{
    public static bool Win;
    public UnityEvent onWin;
    public GameObject coin;
    public GameObject stamp;
    public Animator reportcard;
    public GameObject officeWinStateTrigger;
    public GameObject winConditionHighlight;
    public AudioSource mouthSource;
    public GameObject firstInteraction;
    public static bool highlightActive;
    private void Start()
    {
        if (PlayerPrefs.GetInt("First") == 0)
        {
            PlayerPrefs.SetInt("First", 1);
            firstInteraction.SetActive(true);

        }
        else
        {
            firstInteraction.SetActive(false);
            GetComponent<AudioSource>().enabled = false;
        }
        highlightActive = PlayerPrefs.GetInt("First") == 0;
        if (Win)
        {
            Debug.Log("girdimmi win için?");
            officeWinStateTrigger.SetActive(true);
            winConditionHighlight.SetActive(true);
            FindObjectOfType<CampaignManager>().WinInstance();
            FindObjectOfType<CampaignManager>().EnterScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            onWin.Invoke();
        }
        else
        {
            officeWinStateTrigger.SetActive(false);
            reportcard.gameObject.SetActive(false);
        }
    }
    public void WinState()
    {
        GetComponent<Animator>().SetTrigger("Stamp");
    }
    public void StartOfTheCoin()
    {
        GetComponent<Animator>().SetTrigger("Coin");
    }
    public static bool GetStateOfBeginning()
    {
        return Win;
    }
    public void GiveCoin()
    {
        coin.SetActive(true);
    }
    public void ActivateAnimationOfCoin()
    {
        coin.GetComponent<Animator>().SetTrigger("GiveCoin");
    }
    public void ActivateStamp()
    {
        stamp.SetActive(true);
    }
    public void GiveReportCard()
    {
        reportcard.SetTrigger("GiveReportCard");
        reportcard.gameObject.tag = "grabbableUI";
    }
}
