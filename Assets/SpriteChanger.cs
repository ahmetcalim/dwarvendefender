using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite oculusController;
    public Sprite viveController;
    private void Start()
    {

        switch (FindObjectOfType<VRControllerHandler>().headsetType)
        {
            case VRControllerHandler.VRHeadsetType.HTC:
                GetComponent<SpriteRenderer>().sprite = viveController;
                break;
            case VRControllerHandler.VRHeadsetType.OCULUS:
                GetComponent<SpriteRenderer>().sprite = oculusController;

                break;
            default:
                break;
        }
    }
}
