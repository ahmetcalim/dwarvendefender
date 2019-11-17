using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;
public class ClockBehaviour : MonoBehaviour
{
    public GameObject m;
    public GameObject ThrowTutorialCanvas;
    public GameObject ReportCanvas;
    public Text[] ReportEarningTextBoxes;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Right"))
        {
            if (other.GetComponentInParent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude > 1.5f)
            {
                if (Time.timeScale == 1f)
                {
                    m.SetActive(true);
                    Time.timeScale = 0.01f;
                }
                else
                {
                    m.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("ThrowTutorialDone"))
        {
            PlayerPrefs.SetInt("ThrowTutorialDone", 0);
        }
        if (PlayerPrefs.GetInt("ThrowTutorialDone") == 0)
        {
            ThrowTutorialCanvas.SetActive(true);
            // Do a whole lot of first time stuff
        }

        if (ResourceManager.resourceManager) // Setup the report card to be used if resourceManager exists.
        {
            var r = ResourceManager.resourceManager;
        //    r.EarningDisplays = ReportEarningTextBoxes; // Attach the text boxes.
        }
    }
    

  
    public void StuffThrown()
    {
        if(PlayerPrefs.GetInt("ThrowTutorialDone") == 0)
        {
            ThrowTutorialCanvas.SetActive(false);
            PlayerPrefs.SetInt("ThrowTutorialDone", 1);
        }
    }


}
