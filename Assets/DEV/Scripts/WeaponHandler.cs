using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponHandler : MonoBehaviour
{
    public WeaponThrowingManager throwingManager;
    public List<Rigidbody> weaponRigidbodies;
    public int weaponIndex;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            foreach (var item in weaponRigidbodies)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
    public void ChangeWeapon(int index)
    {
        for (int i = 0; i < weaponRigidbodies.Count; i++)
        {
            weaponRigidbodies[i].gameObject.SetActive(false);
        }
        weaponRigidbodies[index].gameObject.SetActive(true);
        throwingManager.weaponRB = weaponRigidbodies[index];
    }
}
