using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
public class LocomotionRules : MonoBehaviour
{
    
    private SteamVR_Behaviour_Pose pose;
    public Transform otherTransform;
    public bool walkable;
    public bool active { get; set; }
    public bool xAxisMoved { get; set; }
    public LocomotionRules locomotionOther;
    public Rigidbody playerRigidbody;
    public float pow;
    public SteamVR_Action_Pose poseVel;
    public SteamVR_Input_Sources source;
    public bool isOnFloor;
    // Start is called before the first frame update
    void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        
            if (GetVelocityY() > .5f)
            {
                Walk();
            }
            if (GetVelocityY() < .1)
            {
                walkable = false;
                if (!locomotionOther.walkable || !active)
                {
                    playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, Vector3.zero, 2 * Time.deltaTime);

                }
            }
        
        
    }
    
    private void Walk()
    {
       
        if (!walkable)
        {
            walkable = true;
            if (!locomotionOther.active && active)
            {
                playerRigidbody.velocity = new Vector3(poseVel.GetLastVelocity(source).normalized.x, 0f, poseVel.GetLastVelocity(source).normalized.z)   * pow * pose.GetVelocity().magnitude;
            }
        }
    }

    private float GetVelocityY()
    {
        return pose.GetVelocity().y;
    }
    private float GetVelocityX()
    {
        return pose.GetVelocity().x;
    }
    private float GetVelocityZ()
    {
        return pose.GetVelocity().z;
    }
}
