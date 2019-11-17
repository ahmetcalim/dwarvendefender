using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public static float effectSound;
    public GameObject backSoundObject;
    private void Start()
    {
        foreach (var item in backSoundObject.GetComponentsInChildren<AudioSource>())
        {
            item.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
    }
}
