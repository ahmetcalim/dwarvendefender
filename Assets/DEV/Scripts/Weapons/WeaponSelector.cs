using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public void SelectWeapon(int handIndex)
    {
        FindObjectOfType<CampaignManager>().PickWeaponIndex(gameObject, handIndex);
    }
}
