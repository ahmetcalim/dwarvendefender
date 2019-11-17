using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTexts : MonoBehaviour
{
    public float enable;
    public float disable;
    private void Start()
    {
        StartCoroutine(EnableAfterSeconds());
    }
    IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(disable);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    IEnumerator EnableAfterSeconds()
    {
        yield return new WaitForSeconds(enable);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(DisableAfterSeconds());
    }
}
