using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Tooltip("0 = Left, 1 = Right")]
    public int HandIndex;

    private bool hasWeapon;
    public GameObject currentWeapon;
    public Vector3[] snapPositionsRelative;
    /*              LEFT
     * 0:   0,      0.075,      -0.15
     * 1:   0.025,  -0.25,      -0.11
     * 2:   0.03,   0.015,      -0.11
     * 
     * 4:   0.025,  0.15,       -0.075
     * 5:   0.025,  -0.25,      -0.11
     * 6:   0.03,   0.015,      -0.11
     */

    /*              RIGHT
     * 0:  0        0.075       -0.15
     * 1:  0.03     -0.25       0.1
     * 2:  -0.025   -0.03       -0.11
     * 
     * 4:  0.03     0.075       0.15
     * 5:  0.03     -0.25       0.1
     * 6:  -0.025   -0.03       -0.11 
     */
    public Vector3[] snapRotationsRelative;
    /*              LEFT
     * 0:   -25,    -185,       0
     * 1:   90,     0,          270
     * 2:   -110,   -180,       270
     * 
     * 4:   -90,    0,          -90
     * 5:   90,     0,          270
     * 6:   -110,   -180,       270
     */

    /*              RIGHT
     * 0:   -25     0           0
     * 1:   90      0           90
     * 2:   -70     -180        -270
     * 
     * 4:   -90     0           -90
     * 5:   90      0           90
     * 6:   -70     -180        -270
     */

    void Start()
    {
        if (!CampaignManager.campaignManager) return;
        if(CampaignManager.campaignManager._activeWeapons[HandIndex] != 0) // If not zero:
        {
            // Seek the weapon.
            var weps = FindObjectsOfType<Weapon>();
            foreach(Weapon w in weps)
            {
                if(w.index == CampaignManager.campaignManager._activeWeapons[HandIndex]) // If the weapon with the index is found:
                {
                    w.transform.position = transform.position; // Teleport it to the trigger area.
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentWeapon != null && currentWeapon.name == other.gameObject.name)
        {
            return;
        }
        else
        {
            if (other.GetComponent<Weapon>() && currentWeapon == null)
            {
                if (other.GetComponentInParent<GrabberController>())
                {
                    other.GetComponentInParent<GrabberController>().Release();
                }
               
                other.transform.SetParent(null);
                CampaignManager.campaignManager.PickWeaponIndex(other.gameObject, HandIndex);

               
                currentWeapon = other.gameObject;
                int wpnIndex = other.gameObject.GetComponent<Weapon>().index;
                other.transform.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = snapPositionsRelative[wpnIndex];
                other.transform.rotation = Quaternion.Euler(snapRotationsRelative[wpnIndex]);
                GetComponent<AudioSource>().Play();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentWeapon != null && currentWeapon.name != other.gameObject.name)
        {
            return;
           
        }
        else
        {
            if (other.GetComponent<Weapon>())
            {
                currentWeapon = null;
            }
        }
        
    }
}
