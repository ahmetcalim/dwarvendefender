using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialStateChacker : MonoBehaviour
{
    public UnityEvent checkEvent;
    public UnityEvent completed;
    public LoadingBar loadingBar;
    private void OnEnable()
    {
        InvokeEventOnThis();
    }
    public void InvokeEventOnThis()
    {
        checkEvent.Invoke();
    }
    public void Complete()
    {
        completed.Invoke();
    }
}
