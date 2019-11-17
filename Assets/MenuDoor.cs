using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDoor : MonoBehaviour
{
    public static int selectedScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CampaignManager>().LoadInstance(selectedScene);
        }
    }
}
