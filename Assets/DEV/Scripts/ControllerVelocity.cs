using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ControllerVelocity : MonoBehaviour
{
    public SteamVR_Action_Pose actionPose;
    public SteamVR_Input_Sources hand;
    public Vector3 GetVelocity()
    {
        return actionPose.GetLastVelocity(hand);
    }
    public Vector3 GetAngularVelocity()
    {
        return actionPose.GetLastAngularVelocity(hand);
    }
}
