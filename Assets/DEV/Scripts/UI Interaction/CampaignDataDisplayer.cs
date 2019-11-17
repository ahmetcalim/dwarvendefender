using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignDataDisplayer : MonoBehaviour
{
    public GameObject PopupObject;
    public Vector3 Offset;
    public float HoverDistance = 0.03f;
    public Text CampaignDataDisplay;
    [Multiline]
    public string Data;

    public void PopupAt(Transform t)
    {
        PopupObject.SetActive(true);
        // Text Management
        CampaignDataDisplay.text = Data;
    }
    public void PopupDisable()
    {
        PopupObject.SetActive(false);
    }
}
