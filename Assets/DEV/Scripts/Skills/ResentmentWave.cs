using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResentmentWave : MonoBehaviour
{
    // DO THE BURNING THINGS HERE.
    // SINCE THERE'S NO HEALTH SYSTEM IN PLACE, THIS IS EMPTY.

    private float DoT, t;
    
    public void SetSkill(float dot, float dotTime)
    {
        t = dotTime; DoT = dot;
    }
    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 10f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (other.GetComponentInParent<MobParent>().mob)
            {
                StartCoroutine(other.GetComponentInParent<MobParent>().mob.TakeDamageOverTime(DoT, t, .5f));
            }
        }
      
    }
}
