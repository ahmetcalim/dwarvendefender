using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    public Mob mob;
    private void OnEnable()
    {
        if (!mob.puppet.isAlive)
        {
            mob.DestroySelf();
        }
    }
}
