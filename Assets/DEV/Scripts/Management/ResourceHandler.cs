using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public void WriteResources(CurrentPlayer player)
    {
        DataSystem.SavePlayer(player);
    }
    public void AddResource(CurrentPlayer player, int resourceAmount)
    {
        player.resource += resourceAmount;
        WriteResources(player);
    }
}
