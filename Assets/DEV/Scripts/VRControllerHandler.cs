using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

public class VRControllerHandler : MonoBehaviour
{
    //Events
    public int index;
    public UnityEvent onTriggerPressed;
    public UnityEvent onTriggerReleased;
    public UnityEvent onTriggerPressHold;
    public UnityEvent onTouchButtonPressed;
    public UnityEvent onTouchButtonReleased;
    public UnityEvent onTouchButtonPressHold;
    public UnityEvent onGripButtonPressed;
    public UnityEvent onGripButtonReleased;
    public UnityEvent onGripButtonPressHold;
    public UnityEvent walkButtonPressed;
    public UnityEvent walkButtonReleased;
    public UnityEvent walkButtonPressHold;
    //Properties
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean touchPad;
    public SteamVR_Action_Boolean triggerButton;
    public SteamVR_Action_Boolean gripButton;
    public SteamVR_Action_Boolean walkButton;
    public SteamVR_Action_Vibration vibration;
    public bool hapticFeedback;
    public SteamVR_Action_Pose poseAction;
    public Vector2 axis;
    public SteamVR_Action_Vector2 SteamVR_Action_Vector2;

    public Transform player;
    public bool turnActive;

    public enum VRHeadsetType {HTC, OCULUS}
    public VRHeadsetType headsetType;
    public IEnumerator ExecuteHaptic()
    {
        hapticFeedback = true;
        yield return new WaitForSeconds(.1f);
        hapticFeedback = false;
    }

    private void Update()
    {
        if (turnActive)
        {
            axis = SteamVR_Action_Vector2.GetLastAxis(handType);

            if (axis.x == 0)
            {

                CameraFollower.followActive = true;
            }
            else
            {
                if (axis.x > 0.1f)
                {
                    CameraFollower.followActive = false;
                    player.Rotate(Vector3.up, axis.x);
                }
                else if (axis.x < -0.1f)
                {
                    CameraFollower.followActive = false;
                    player.Rotate(Vector3.down, axis.x);

                }
            }
        }
      
      
      

        
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (poseAction.GetLastVelocity(handType).magnitude >= 9f)
            {
                SetDamageLevelOfWeapon(Weapon.DamageLevel.MAX, 4);
            }
            else if (poseAction.GetLastVelocity(handType).magnitude < 9f && poseAction.GetLastVelocity(handType).magnitude >= 7f)
            {
                SetDamageLevelOfWeapon(Weapon.DamageLevel.HIGH, 3);
            }
            else if (poseAction.GetLastVelocity(handType).magnitude < 7f && poseAction.GetLastVelocity(handType).magnitude >= 3.5f)
            {
                SetDamageLevelOfWeapon(Weapon.DamageLevel.MED, 2);
            }
            else if (poseAction.GetLastVelocity(handType).magnitude < 3.5f && poseAction.GetLastVelocity(handType).magnitude >= 1.5f)
            {
                SetDamageLevelOfWeapon(Weapon.DamageLevel.LOW, 1);
            }
            else
            {
                SetDamageLevelOfWeapon(Weapon.DamageLevel.MIN, 0);
            }
        }
        if (triggerButton.GetState(handType)) TriggerPressHold();
        if (touchPad.GetState(handType)) TouchPadPresHold();
        if (gripButton.GetState(handType))
        {
            OnGripButtonPressedHold();

        }
        if (walkButton.GetState(handType))
        {
            OnWalkButtonPressHold();
        }

       
        if (hapticFeedback)
        {
           
            vibration.Execute(.1f, .1f, 320, 1f, handType);
            vibration.Execute(.1f, .1f, 320, 1f, handType);
        }
            
    }
    private void SetDamageLevelOfWeapon(Weapon.DamageLevel damageLevel, int damageLevelIndex)
    {
        if (GetComponent<WeaponThrowingManager>())
        {
            if (GetComponent<WeaponThrowingManager>().weaponRB)
            {
                GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().damageLevel = damageLevel;
                GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().DamageLevelIndex = damageLevelIndex;

            }
        }
       
      
    }
    private void Start()
    {
        if (!pose)
        {
            pose = GetComponent<SteamVR_Behaviour_Pose>();
        }
        if (XRDevice.model.Contains("Vive"))
        {
            headsetType = VRHeadsetType.HTC;
        }
        else
        {
            headsetType = VRHeadsetType.OCULUS;
        }

        triggerButton.AddOnStateDownListener(TriggerPressed, handType);
        triggerButton.AddOnStateUpListener(TriggerUp, handType);
        touchPad.AddOnStateDownListener(TouchPadPressed, handType);
        touchPad.AddOnStateUpListener(TouchPadReleased, handType);
        gripButton.AddOnStateDownListener(OnGripButtonPressed, handType);
        gripButton.AddOnStateUpListener(OnGripButtonReleased, handType);
        walkButton.AddOnStateDownListener(OnWalkButtonPressed, handType);
        walkButton.AddOnStateUpListener(OnWalkButtonReleased, handType);
    }
    
    private void TriggerPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
         onTriggerPressed.Invoke();
    }
    private void TriggerUp(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
          onTriggerReleased.Invoke();
    }
    private void TriggerPressHold()
    {
        onTriggerPressHold.Invoke();
    }
    private void TouchPadPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
          onTouchButtonPressed.Invoke();
    }
    private void TouchPadReleased(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        onTouchButtonReleased.Invoke();
    }
    private void TouchPadPresHold()
    {
        onTouchButtonPressHold.Invoke();
    }
    private void OnGripButtonPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
         onGripButtonPressed.Invoke();
    }
    private void OnGripButtonPressedHold()
    {
         onGripButtonPressHold.Invoke();
    }
    private void OnGripButtonReleased(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
         onGripButtonReleased.Invoke();
    }
    private void OnWalkButtonPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        walkButtonPressed.Invoke();
    }
    private void OnWalkButtonReleased(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        walkButtonReleased.Invoke();
    }
    private void OnWalkButtonPressHold()
    {
        walkButtonPressHold.Invoke(); ;
    }
}
