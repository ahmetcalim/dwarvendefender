using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class JoystickMover : MonoBehaviour
{
    public bool joystick;
    public SteamVR_Action_Vector2 axis;
    public Rigidbody playerRigidbody; //Camera-Rig
    public SteamVR_Input_Sources source;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (joystick)
        {
            Vector2 ax = axis.GetLastAxis(source);
            playerRigidbody.velocity = (Camera.main.transform.forward * ax.y * speed) + Camera.main.transform.right * ax.x * speed;
        }
    }
}
