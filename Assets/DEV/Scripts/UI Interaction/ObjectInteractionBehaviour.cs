using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObjectInteractionBehaviour : MonoBehaviour
{
    public bool restart;
    public bool goMenu;
    public bool settings;
    public bool resume;
    public GameObject hoverText;
    public void Restart()
    {
        if (CampaignManager.campaignManager)
        {
            CampaignManager.campaignManager.ReloadInstance();
        }
      
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoMenu()
    {
        CampaignManager.campaignManager.ChangeScene(0);
        // SceneManager.LoadScene(0);
    }
    public void DoEventByBool()
    {
        if (restart)
        {
            Restart();
        }
        else if (goMenu)
        {
            GoMenu();
        }
        else if (resume)
        {
            GetComponentInParent<SteamObjectContainer>().Resume();
        }
    }
}
