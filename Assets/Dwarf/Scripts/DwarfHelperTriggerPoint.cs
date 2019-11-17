using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfHelperTriggerPoint : MonoBehaviour
{
    DwarfHelperAI dwarfHelperAI;
    public AudioClip clip;
    private void OnEnable()
    {
        dwarfHelperAI = FindObjectOfType<DwarfHelperAI>();
    }

    public void Complete()
    {
        StartCoroutine(dwarfHelperAI.Congratulations());
    }
}

