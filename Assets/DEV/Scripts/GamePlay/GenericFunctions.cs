using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
public class GenericFunctions : MonoBehaviour
{

    private bool hapticFeedback;

    public SteamVR_Action_Vibration vibration;
   
    public IEnumerator ExecuteHaptic(SteamVR_Input_Sources source)
    {
        hapticFeedback = true;
        StartCoroutine(ExecuteHapticFeedback(source));
        yield return new WaitForSeconds(.5f);
        hapticFeedback = false;
    }
    // Update is called once per frame
    IEnumerator ExecuteHapticFeedback(SteamVR_Input_Sources source)
    {
        yield return new WaitForSecondsRealtime(.02f);
        if (hapticFeedback)
        {
            vibration.Execute(1f, .5f, 1000f, 5f, source);
            StartCoroutine(ExecuteHapticFeedback(source));
        }
    }
    public void OpenSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
