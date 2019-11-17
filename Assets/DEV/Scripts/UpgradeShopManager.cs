using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeShopManager : MonoBehaviour {

    public RawImage WeaponImage;
    public Text WeaponNameTextBox;
    public Text weaponNameFirstChar;
    public Text WeaponDescriptionTextBox;

    public Text GoldTextBox;
    public Text RuneTextBox;
    public Text PageNumberTextBox;

    public Button[] UpgradeButtons;
    public Image[] UpgradeIcons;
    public Image[] LockIcons;
    public Text[] UpgradeNameTextBoxes;
    public Text[] UpgradeCostTextBoxes;
    public Image[] UpgradeCostIcons;

    public Button ConfirmButton;

    public Weapon activeWeapon;
    public Upgrade[] activeUpgrades;

    public UnityEvent OnUpgradePurchased;
    public Sprite LockedIcon;
    public Sprite UnlockedIcon;

    public Animator PageAnimator;
    public GameObject UpgradeCanvas;
    public GameObject BookPages;

    public int WeaponCount = 1;
    int CurrentWeapon = 0;
    int CurrentUpgrade = -1;

    private float _flippedAngle = 0;

    public string newName;

    public AudioClip flipClip;
    public AudioClip upgradePurchasedClip;
    public IEnumerator FlipAnimation(bool ToTheRight)
    {
        GetComponent<AudioSource>().PlayOneShot(flipClip);
        PageAnimator.ResetTrigger("TurnLeft");
        PageAnimator.ResetTrigger("TurnRight");
        if (ToTheRight)
            PageAnimator.SetTrigger("TurnRight");
        else
            PageAnimator.SetTrigger("TurnLeft");
        UpgradeCanvas.SetActive(false);
        _flippedAngle = 1;
        yield return new WaitForSeconds(1);
        UpgradeCanvas.SetActive(true);
        if (ToTheRight) PreviousWeapon();
        else NextWeapon();
        _flippedAngle = 0;
    }

    public IEnumerator ToggleOpen()
    {
        GetComponent<Animator>().SetTrigger("OpenBook");
        yield return new WaitForSeconds(1.5f);
        activeWeapon = UpgradeManager.upgradeManager.Weapons[CurrentWeapon];
        activeUpgrades = UpgradeManager.upgradeManager.GetUpgradesByIndex(CurrentWeapon);
        UpgradeCanvas.SetActive(true);
        BookPages.SetActive(true);
        RefreshDisplay();
    }

    public void StartFlip(bool ToTheRight)
    {
        if (_flippedAngle != 0) return; // if already flipping, return.
        if (ToTheRight) // position panel on left
        {
            if (CurrentWeapon <= 0) return; // if can't flip, return.
            //FlippingPage.transform.rotation = LeftPage.rotation;
        }
        else // position panel on right.
        {
            if(CurrentWeapon + 1 >= UpgradeManager.upgradeManager.Weapons.Length) return; // if can't flip, return.
            //FlippingPage.transform.rotation = RightPage.rotation;
        }
        //FlippingPage.SetActive(true);
        StartCoroutine(FlipAnimation(ToTheRight));
    }

    public void NextWeapon()
    {
        if(UpgradeManager.upgradeManager.Weapons.Length > CurrentWeapon + 1)
        {
            CurrentWeapon += 1;
            activeWeapon = UpgradeManager.upgradeManager.Weapons[CurrentWeapon];
            activeUpgrades = UpgradeManager.upgradeManager.GetUpgradesByIndex(CurrentWeapon);
            RefreshDisplay();
        }
        WeaponImage.gameObject.GetComponent<Button>().Select();
    }

    public void PreviousWeapon()
    {
        if(CurrentWeapon > 0)
        {
            CurrentWeapon -= 1;
            activeWeapon = UpgradeManager.upgradeManager.Weapons[CurrentWeapon];
            activeUpgrades = UpgradeManager.upgradeManager.GetUpgradesByIndex(CurrentWeapon);
            RefreshDisplay();
        }
        WeaponImage.gameObject.GetComponent<Button>().Select();
    }
    private void LateUpdate()
    {
        GoldTextBox.text = ResourceManager.resourceManager.ResourceAmounts[(int)ResourceTypes.Coins].ToString();
        RuneTextBox.text = ResourceManager.resourceManager.ResourceAmounts[(int)ResourceTypes.RuneStones].ToString();
    }

    public void RefreshDisplay()
    {
        CurrentUpgrade = -1;
        GoldTextBox.text = ResourceManager.resourceManager.ResourceAmounts[(int)ResourceTypes.Coins].ToString();
        RuneTextBox.text = ResourceManager.resourceManager.ResourceAmounts[(int)ResourceTypes.RuneStones].ToString();
        activeWeapon.WeaponUnlocked = UpgradeManager.upgradeManager.WeaponUnlocks[CurrentWeapon];
        WeaponImage.texture = activeWeapon.WeaponSprite;
        newName = activeWeapon.WeaponName.Substring(1);
        WeaponNameTextBox.text = newName;
        char[] aa = activeWeapon.WeaponName.ToCharArray();
        weaponNameFirstChar.text = aa[0].ToString();
        WeaponDescriptionTextBox.text = activeWeapon.WeaponDescription;
        
        for(int i = 0; i < UpgradeManager.upgradeManager.UpgradesPerWeapon; i++)
        {
            if (UpgradeIcons[i])
            {
                UpgradeNameTextBoxes[i].text = activeUpgrades[i].UpgradeName;

                UpgradeIcons[i].sprite = null;
                UpgradeIcons[i].sprite = activeUpgrades[i].UpgradeIcon;
                LockIcons[i].sprite = null;
                if (activeUpgrades[i].UpgradeLevel == 0)
                {
                    LockIcons[i].sprite = LockedIcon;
                }
                else
                {
                    LockIcons[i].sprite = UnlockedIcon;
                }

                UpgradeCostIcons[i].enabled = true;
                UpgradeCostIcons[i].sprite = null;
                int lvl = activeUpgrades[i].UpgradeLevel;
                if(lvl == activeUpgrades[i].UpgradeCosts.Length)
                {
                    UpgradeCostIcons[i].enabled = false;
                    UpgradeCostTextBoxes[i].text = "MAX";
                }
                else
                {
                    UpgradeCostTextBoxes[i].text = activeUpgrades[i].UpgradeCosts[lvl].ToString();
                    UpgradeCostIcons[i].sprite = ResourceManager.resourceManager.ResourceSprites[(int)activeUpgrades[i].UpgradeCostTypes[lvl]];
                }
            }
        }
        PageNumberTextBox.text = (CurrentWeapon + 1).ToString() + " / " + WeaponCount.ToString();

        if (UpgradeManager.upgradeManager.WeaponUnlocks[activeWeapon.upgradeIndex]) // If weapon is unlocked:
        {
            ConfirmButton.interactable = false;
            ConfirmButton.GetComponentInChildren<Text>().text = "ALREADY UNLOCKED";

            foreach (Button b in UpgradeButtons)
            {
                if (b)
                {
                    b.interactable = true;
                }
            }
        }
        else
        {
            ConfirmButton.interactable = true;
            ConfirmButton.GetComponentInChildren<Text>().text = "UNLOCK: " + activeWeapon.WeaponCost.ToString() + " COINS";
            foreach (Button b in UpgradeButtons)
            {
                b.interactable = false;
            }
        }
    }

    public void ChangeDescription(int i)
    {
        if (i == -1)
        {
            RefreshDisplay();
        }
        else if(UpgradeButtons[i])
        {
            var u = activeUpgrades[i];
            CurrentUpgrade = i;
            WeaponDescriptionTextBox.text = u.UpgradeDescription;
            WeaponNameTextBox.text = u.UpgradeName + " (Level " + u.UpgradeLevel.ToString() + ")";

            ConfirmButton.interactable = true;
            if (u.UpgradeLevel == u.UpgradeCosts.Length)
            {
                ConfirmButton.interactable = false;
                ConfirmButton.GetComponentInChildren<Text>().text = "MAXED OUT";
            }
            else
            {
                string resourceName = ResourceManager.resourceManager.ResourceNames[(int)u.UpgradeCostTypes[u.UpgradeLevel]];
                int resourceAmount = ResourceManager.resourceManager.ResourceAmounts[(int)u.UpgradeCostTypes[u.UpgradeLevel]];
                if (!u.Unlocked)
                {
                    ConfirmButton.GetComponentInChildren<Text>().text = "Unlock: " + u.UpgradeCosts[u.UpgradeLevel].ToString() + " " + resourceName;
                }
                else
                {
                    ConfirmButton.GetComponentInChildren<Text>().text = "Upgrade: " + u.UpgradeCosts[u.UpgradeLevel].ToString() + " " + resourceName;
                }
            }
        }
        else
        {
            var u = activeUpgrades[i];
            WeaponDescriptionTextBox.text = u.UpgradeDescription;
            WeaponNameTextBox.text = u.UpgradeName;
            ConfirmButton.GetComponentInChildren<Text>().text = "Coming soon.";
        }
    }
    

	// Use this for initialization
	void Start () {
        StartCoroutine(ToggleOpen());
        
    }

    public void UpgradePurchased()
    {
        if (CurrentUpgrade == -1) // if weapon itself is picked.
        {
            if (UpgradeManager.upgradeManager.WeaponUnlocks[activeWeapon.upgradeIndex]) return; // if already unlocked, return.
            else if (ResourceManager.resourceManager.ResourceAmounts[0] < activeWeapon.WeaponCost) return; // not enough monies.
            ResourceManager.resourceManager.ResourceAmounts[0] -= activeWeapon.WeaponCost; // reduce the cost.
            UpgradeManager.upgradeManager.WeaponUnlocks[activeWeapon.upgradeIndex] = true; // unlock the weapon.
            activeWeapon.WeaponUnlocked = true;
            OnUpgradePurchased.Invoke(); // Invoke extras.
            RefreshDisplay(); // Refresh the display so the buttons are all proper.
            UpgradeManager.upgradeManager.CheckWeaponUnlocks(); // Make sure the model is unlocked for use.
            return; // End weapon purchase.
        }
        var u = activeUpgrades[CurrentUpgrade];
        if (u.UpgradeLevel == u.UpgradeCosts.Length) return;
        int targetLevel = u.UpgradeLevel;
        int cost = u.UpgradeCosts[targetLevel];
        int resourceIndex = (int)(u.UpgradeCostTypes[targetLevel]);
        if (ResourceManager.resourceManager.ResourceAmounts[resourceIndex] >= cost) // Upgrade purchased!
        {
            ResourceManager.resourceManager.ResourceAmounts[resourceIndex] -= cost;
            int mngIndex = CurrentWeapon * UpgradeManager.upgradeManager.UpgradesPerWeapon + CurrentUpgrade;
            // level up on manager
            UpgradeManager.upgradeManager.Upgrades[mngIndex].UpgradeLevel++;

            OnUpgradePurchased.Invoke(); // Invoke extra stuff?

            //Refresh display.
            int c = CurrentUpgrade;
            RefreshDisplay();
            ChangeDescription(c);
        }
        GetComponent<AudioSource>().PlayOneShot(upgradePurchasedClip);
    }

}


