using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsFixer : MonoBehaviour
{
    public Transform hips;
    public Animator anim;
    void OnEnable()
    {
     
        StartCoroutine(FixHips());
    }
    public IEnumerator FixHips()
    {
        yield return new WaitForSeconds(1f);
    }
    private void LateUpdate()
    {
       
    }
}
