using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public List<Item> weapons;
    public List<Item> orbs;
    public CurrentPlayer player;
    private void Start()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (player.weapons[i] == 1)
            {
                weapons[i].storeState = Item.StoreState.PURCHASED;
            }
        }
    }
    public void Purchase(Item item)
    {
        player.weapons[item.wIndex] = 1;
        player.resource -= item.cost;
        item.storeState = Item.StoreState.PURCHASED;
        player.SavePlayer();
    }
}
