using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShatterBehaviour : MonoBehaviour
{
    public List<GameObject> colliders;
    private int currentCollider = 0;
    private void OnEnable()
    {
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f/11f);
       
        if (currentCollider < colliders.Count)
        {
            currentCollider++;
            colliders[currentCollider - 1].SetActive(true);
            StartCoroutine(Explosion());
        }
       
    }
}