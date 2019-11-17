using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrilledTrigger : MonoBehaviour
{
    Drilled drilled;
    private bool _beheaded = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SeekManager());
    }

    private void OnTriggerStay(Collider collider)
    {
        if (_beheaded) return; // already beheaded
        if (!drilled) return; // no manager
        if (!collider.gameObject.CompareTag("head")) return; // not hit by sword
        if (!collider.GetComponentInParent<Mob>()) return; // can't find mob tracker
        if (!collider.GetComponentInParent<Mob>().sliced) return; // still alive
        drilled.AddSword();
        _beheaded = true;
    }

    private IEnumerator SeekManager()
    {
        if (FindObjectOfType<Drilled>()) drilled = FindObjectOfType<Drilled>();
        yield return new WaitForSeconds(1.0f);
        if (!drilled) StartCoroutine(SeekManager());
    }
}
