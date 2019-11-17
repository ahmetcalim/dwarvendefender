using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class SettingsManager : MonoBehaviour
{
    public void SetQuality(float qualityAmount)
    {
        XRSettings.eyeTextureResolutionScale = qualityAmount;
    }

}
