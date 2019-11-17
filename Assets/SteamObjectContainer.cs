using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SteamObjectContainer : MonoBehaviour
{
    public static WeaponHandler leftHandler;
    public static WeaponHandler rigtHandler;
    public WeaponHandler _leftHandler;
    public WeaponHandler _rightHandler;
    public GameObject menuObject;
    public GameObject fingerHand;
    public GameObject normalHand;
    public Animator handAnimator;
    public GameObject watch;
   
    private void Awake()
    {
        leftHandler = _leftHandler;
        rigtHandler = _rightHandler;
    }
    public void Stop()
    {


    }
    public void Pause()
    {
        handAnimator.SetTrigger("pointer");
        StartCoroutine(SetTimeScaleAfterSeconds(handAnimator.GetCurrentAnimatorStateInfo(0).length));
        watch.SetActive(false);
        //
        
        menuObject.SetActive(true);
        if (_leftHandler.GetComponent<WeaponThrowingManager>().weaponRB)
        {
            if (_leftHandler.GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().throwable)
            {
                _leftHandler.GetComponent<WeaponThrowingManager>().weaponRB.gameObject.SetActive(false);
            }

        }
        if (_rightHandler.GetComponent<WeaponThrowingManager>().weaponRB)
        {
            if (_rightHandler.GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().throwable)
            {
                _rightHandler.GetComponent<WeaponThrowingManager>().weaponRB.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator SetTimeScaleAfterSeconds(float duringTime)
    {
        yield return new WaitForSeconds(duringTime);
        Time.timeScale = .0001f;
    }
    public void Resume()
    {
        if (_leftHandler.GetComponent<WeaponThrowingManager>().weaponRB )
        {
            if (_leftHandler.GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().throwable)
            {
                _leftHandler.GetComponent<WeaponThrowingManager>().weaponRB.gameObject.SetActive(true);
            }
           
        }
        if (_rightHandler.GetComponent<WeaponThrowingManager>().weaponRB)
        {
            if (_rightHandler.GetComponent<WeaponThrowingManager>().weaponRB.GetComponent<Weapon>().throwable)
            {
                _rightHandler.GetComponent<WeaponThrowingManager>().weaponRB.gameObject.SetActive(true);
            }
        }
        Time.timeScale = 1f;
        watch.SetActive(true);
        handAnimator.SetTrigger("holding_weapon");
        menuObject.SetActive(false);
    }
}
