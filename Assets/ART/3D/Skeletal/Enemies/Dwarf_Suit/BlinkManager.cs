using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkManager : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        StartCoroutine(BlinkIt());
    }

    IEnumerator BlinkIt()
    {
        yield return new WaitForSeconds(Random.Range(2f,3.5f));
        anim.SetTrigger("Blink");
        StartCoroutine(BlinkIt());
    }
}
