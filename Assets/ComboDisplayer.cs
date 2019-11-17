using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboDisplayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!ComboTracker.comboTracker) return;
        if (ComboTracker.comboTracker.comboCounter > 1)
        {
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Slider", ComboTracker.comboTracker._comboTimer / ComboTracker.comboTracker.Timers[ComboTracker.comboTracker.comboCounter]);
        }
        else
        {
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Slider", 0f);
        }
        
    }
}
