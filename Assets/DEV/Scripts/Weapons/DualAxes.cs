using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualAxes : Weapon
{
    private void Start()
    {
        foreach(Retribution r in FindObjectsOfType<Retribution>())
        {
            if (r.RetributionWave.GetComponent<ResentmentWave>())
                Skills[0] = r;
            else if (r.RetributionWave.GetComponent<RetributionWave>())
                Skills[1] = r;
        }
        Skills[2] = GetComponentInChildren<CleaveBlade>();

        StartCoroutine(SeekUpgrades(0.1f, 1));

    }
}
