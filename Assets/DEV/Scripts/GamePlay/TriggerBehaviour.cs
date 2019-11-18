using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBehaviour : MonoBehaviour
{
    public UnityEvent onHit;
    public string tag;
    public bool DoubleTap = false;
    public float TapTime = 0.5f;
    private float _tapTimer = 0;
    public bool watch;
    private int hitCount;
    private void OnTriggerEnter(Collider other)
    {
        bool watchCon = other.GetComponent<InteractableWithWatch>() || other.GetComponentInParent<InteractableWithWatch>()
            || other.GetComponentInChildren<InteractableWithWatch>();
        watchCon = watchCon && watch;

        bool tagCon = !watch && other.CompareTag(tag);
     
        if (tagCon || watchCon)
        {
          
            if (!DoubleTap)
            {
                if (other.CompareTag("EffectActivator"))
                {
                    if (GetComponentInParent<VRControllerHandler>())
                    {
                        StartCoroutine(GetComponentInParent<VRControllerHandler>().ExecuteHaptic());
                    }
                 
                }
                onHit.Invoke();
            }
            else if(_tapTimer > 0)
            {
                hitCount++;
                if (hitCount >= 2)
                {
                    if (other.CompareTag("EffectActivator"))
                    {
                        StartCoroutine(GetComponentInParent<VRControllerHandler>().ExecuteHaptic());
                    }
                    onHit.Invoke();
                }
                _tapTimer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool watchCon = other.GetComponent<InteractableWithWatch>() || other.GetComponentInParent<InteractableWithWatch>()
            || other.GetComponentInChildren<InteractableWithWatch>();
        watchCon = watchCon && watch;
        bool tagCon = !watch && other.CompareTag(tag);
        if ( (watchCon || tagCon) 
            && DoubleTap 
            && _tapTimer == 0)
        {
            _tapTimer = TapTime;
        }
    }

    private void Update()
    {
        if(_tapTimer > 0)
        {
            _tapTimer -= Time.deltaTime;
            if (_tapTimer < 0)
            {
                _tapTimer = 0;
                hitCount = 0;
            } 
        }
    }
}
