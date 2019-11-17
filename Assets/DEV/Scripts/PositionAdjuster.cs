using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR;
public class PositionAdjuster : MonoBehaviour
{
    private Transform cameraTransform;
    // Start is called before the first frame update
    private void Awake()
    {
        XRSettings.eyeTextureResolutionScale = 1f;
    }
   
}
