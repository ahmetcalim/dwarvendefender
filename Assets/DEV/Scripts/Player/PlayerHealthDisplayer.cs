using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDisplayer : MonoBehaviour
{
    void Update()
    {
        GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Slider", FindObjectOfType<PlayerHealthTracker>().Health / FindObjectOfType<PlayerHealthTracker>().MaxHealth);
    }
}
