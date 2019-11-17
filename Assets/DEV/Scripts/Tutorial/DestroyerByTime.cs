using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyerByTime : MonoBehaviour
{
    public float duration;
    public UnityEvent onDisabled;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyByTime());
    }
    IEnumerator DestroyByTime()
    {
        GetComponent<TutorialStateChacker>().loadingBar.GetComponent<Animator>().SetTrigger("Load");
        yield return new WaitForSeconds(duration);
        onDisabled.Invoke();
    }
}
