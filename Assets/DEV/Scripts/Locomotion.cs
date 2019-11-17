using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public LocomotionRules leftLocomotion;
    public LocomotionRules rightLocomotion;
    public Rigidbody playerRB;
    private float frames;
    private float avVelocity;
    private int counter = 0;
    public float multiplier;
    public bool menu;
    
    private void Update()
    {
        if (leftLocomotion.gameObject.activeInHierarchy && rightLocomotion.gameObject.activeInHierarchy)
        {
            if (frames > 1)
            {
                avVelocity /= (counter * 2);
                counter = 0;
                frames = 0;
            }
            else
            {
                counter++;
                avVelocity += (leftLocomotion.poseVel.GetLastVelocity(leftLocomotion.source).magnitude + rightLocomotion.poseVel.GetLastVelocity(rightLocomotion.source).magnitude) / (counter * 2);

                frames += Time.deltaTime;
            }
        }
    
    }
    public void Walk()
    {
        if (!leftLocomotion.gameObject.activeSelf || !rightLocomotion.gameObject.activeSelf || Vector3.Distance(leftLocomotion.transform.position, rightLocomotion.transform.position) > 4) return;


            if (leftLocomotion.poseVel.GetLastVelocity(leftLocomotion.source).y > .3f && rightLocomotion.poseVel.GetLastVelocity(rightLocomotion.source).y > .3f)
        {
            return;
        }
        else
        {
            if (!menu)
            {
                if ((!leftLocomotion.walkable && rightLocomotion.walkable) && (rightLocomotion.active && leftLocomotion.active))
                {
                    Vector3 a = (new Vector3(leftLocomotion.transform.forward.x, 0f, leftLocomotion.transform.forward.z) + new Vector3(rightLocomotion.transform.forward.x, 0f, rightLocomotion.transform.forward.z)) / 2f;
                    Vector3 vel = (a * (Mathf.Log(rightLocomotion.poseVel.GetLastVelocity(rightLocomotion.source).magnitude + 1.0f, 4) + 4)) * multiplier;
                    playerRB.velocity = new Vector3(vel.x, playerRB.velocity.y, vel.z);
                }
                if ((leftLocomotion.walkable && !rightLocomotion.walkable) && (rightLocomotion.active && leftLocomotion.active))
                {
                    Vector3 a = (new Vector3(leftLocomotion.transform.forward.x, 0f, leftLocomotion.transform.forward.z) + new Vector3(rightLocomotion.transform.forward.x, 0f, rightLocomotion.transform.forward.z)) / 2f;
                    Vector3 vel = (a * (Mathf.Log(leftLocomotion.poseVel.GetLastVelocity(leftLocomotion.source).magnitude + 1.0f, 4) + 4)) * multiplier;
                    playerRB.velocity = new Vector3(vel.x, playerRB.velocity.y, vel.z);
                }
            }
            else
            {
                if ((!leftLocomotion.walkable && rightLocomotion.walkable) && (rightLocomotion.active && leftLocomotion.active))
                {
                    Vector3 a = (new Vector3(leftLocomotion.transform.forward.x, 0f, leftLocomotion.transform.forward.z) + new Vector3(rightLocomotion.transform.forward.x, 0f, rightLocomotion.transform.forward.z)) / 2f;
                    Vector3 vel = a *  multiplier * Time.deltaTime;
                    playerRB.velocity = new Vector3(vel.x, playerRB.velocity.y, vel.z);

                }
                if ((leftLocomotion.walkable && !rightLocomotion.walkable) && (rightLocomotion.active && leftLocomotion.active))
                {
                    Vector3 a = (new Vector3(leftLocomotion.transform.forward.x, 0f, leftLocomotion.transform.forward.z) + new Vector3(rightLocomotion.transform.forward.x, 0f, rightLocomotion.transform.forward.z)) / 2f;
                    Vector3 vel = a * multiplier * Time.deltaTime;
                    playerRB.velocity = new Vector3(vel.x, playerRB.velocity.y, vel.z); ;
                }
            }
        }
      

    }
    public void Stop()
    {
        playerRB.velocity = Vector3.zero;
    }
    private static Vector3 RoundVector3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
}
