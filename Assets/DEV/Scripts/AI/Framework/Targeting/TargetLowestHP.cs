using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Targeting/Lowest HP")]
public class TargetLowestHP : AITargeting
{
    public override GameObject Scan(AIStateMachine fsm)
    {
        // Variable preparation.
        GameObject lowest = null;
        float lowestHP = Mathf.Infinity;

        foreach (string scriptName in TypeNames) // For each type specified:
        {
            Type targetType = System.Reflection.Assembly.GetExecutingAssembly().GetType(scriptName);
            var targets = FindObjectsOfType(targetType);
            if (targets == null || targets.Length == 0) return null;
            foreach (var target in targets) // For each target gathered:
            {
                GameObject go = GameObject.Find(target.name);
                if (!go.activeInHierarchy) continue;
                if (!StateControl(go)) continue;
                // See if they have a health component.
                // If they do, compare with current lowest health.
                // If they are lower than the lowest, keep track of them.
                if (go.GetComponent<Mob>())
                {
                    if(go.GetComponent<Mob>().hitPoint < lowestHP)
                    {
                        lowestHP = go.GetComponent<Mob>().hitPoint;
                        lowest = go;
                    }
                }
                else if (go.GetComponent<PlayerHealthTracker>())
                {
                    if (go.GetComponent<PlayerHealthTracker>().Health < lowestHP)
                    {
                        lowestHP = go.GetComponent<PlayerHealthTracker>().Health;
                        lowest = go;
                    }
                }
                else if (go.GetComponent<Spire>())
                {
                    if (go.GetComponent<Spire>().hp < lowestHP)
                    {
                        lowestHP = go.GetComponent<Spire>().hp;
                        lowest = go;
                    }
                }
            }
        }
        return lowest;
    }
}
