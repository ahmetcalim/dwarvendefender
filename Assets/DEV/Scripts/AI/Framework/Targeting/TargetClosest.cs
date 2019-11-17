using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Targeting/Closest")]
public class TargetClosest : AITargeting
{
    public override GameObject Scan(AIStateMachine fsm)
    {
        GameObject closest = null;
        foreach (string scriptName in TypeNames) // For each type specified:
        {
            // Gather targets of type.
            Type targetType = System.Reflection.Assembly.GetExecutingAssembly().GetType(scriptName);
            var targets = FindObjectsOfType(targetType);
            if (targets == null || targets.Length == 0) return null;
            foreach(var target in targets) // For each target gathered:
            {
                GameObject go = GameObject.Find(target.name);
                if (!go.activeInHierarchy) continue;
                if (!StateControl(go)) continue;
                // Compare distances with the current closest.
                // If target is closer than current closest, keep track of target.
                if (closest == null)
                    closest = go;
                else if (Vector3.Distance(go.transform.position, fsm.transform.position) < Vector3.Distance(closest.transform.position, fsm.transform.position))
                    closest = go;
            }
        }
        return closest;
    }
}
