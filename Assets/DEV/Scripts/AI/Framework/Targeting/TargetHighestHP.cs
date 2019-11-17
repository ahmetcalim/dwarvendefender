using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Targeting/Highest HP")]
public class TargetHighestHP : AITargeting
{
    public override GameObject Scan(AIStateMachine fsm)
    {
        // Variable preparation.
        GameObject highest = null;
        float highestHP = Mathf.NegativeInfinity;

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
                // If they do, compare with current highest health.
                // If they are higher than the highest, keep track of them.
                if (go.GetComponent<Mob>())
                {
                    if (go.GetComponent<Mob>().hitPoint > highestHP)
                    {
                        highestHP = go.GetComponent<Mob>().hitPoint;
                        highest = go;
                    }
                }
                else if (go.GetComponent<PlayerHealthTracker>())
                {
                    if (go.GetComponent<PlayerHealthTracker>().Health > highestHP)
                    {
                        highestHP = go.GetComponent<PlayerHealthTracker>().Health;
                        highest = go;
                    }
                }
                else if (go.GetComponent<Spire>())
                {
                    if (go.GetComponent<Spire>().hp > highestHP)
                    {
                        highestHP = go.GetComponent<Spire>().hp;
                        highest = go;
                    }
                }
            }
        }
        return highest;
    }
}
