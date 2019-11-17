using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuWeaponDescription : MonoBehaviour
{
    public WeaponThrowingManager weaponThrowingManagerLeft;
    public WeaponThrowingManager weaponThrowingManagerRight;
    private Weapon weaponLeft;
    private Weapon weaponRight;

    public RawImage weaponImageLeft;
    public RawImage weaponImageRight;
    public Text leftName;
    public Text rightName;
    public Text leftDescription;
    public Text rightDescription;
    private void OnEnable()
    {
        weaponLeft = weaponThrowingManagerLeft.weaponRB.GetComponent<Weapon>();
        weaponImageLeft.texture = weaponLeft.WeaponSprite;
        weaponRight = weaponThrowingManagerRight.weaponRB.GetComponent<Weapon>();
        weaponImageRight.texture = weaponRight.WeaponSprite;
        leftName.text = weaponLeft.WeaponName;
        rightName.text = weaponRight.WeaponName;
    }
}
