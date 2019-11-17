using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipseSlamVFXActivator : MonoBehaviour
{
    public GameObject MainEffect;
    public Transform AttachPoint;
    public float Effect_DestroyTime = 10;

    [HideInInspector] public bool IsMobile;
    public void ActivateEffect()
    {
        if (MainEffect == null) return;
        var instance = Instantiate(MainEffect, AttachPoint.transform.position - new Vector3(0f, .2f, 0f), AttachPoint.transform.rotation);
        if (Effect_DestroyTime > 0.01f) Destroy(instance, Effect_DestroyTime);
    }
    
}
