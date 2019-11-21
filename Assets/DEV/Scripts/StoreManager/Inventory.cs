using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> weapons;
    public List<Item> orbs;
    public Store store;
    public CurrentPlayer player;
    private void LateUpdate()
    {
        UpdateInventory();
    }
    void UpdateInventory()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].nameText.text = store.weapons[i].name;
            SetItemState(player.weapons[i] == 1, i);
        }
    }
    public void SetItemState(bool state, int i)
    {
        weapons[i].gameObject.SetActive(state);
    }
}
