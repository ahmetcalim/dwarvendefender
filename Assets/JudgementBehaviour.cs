using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementBehaviour : MonoBehaviour
{
    public List<GameObject> flaks;
    public GameObject fireLine;
    private Mob currentMob;
    private void OnCollisionEnter(Collision other)
    {
        if (!flaks[0].activeSelf)
        {
            for (int i = 0; i < flaks.Count; i++)
            {
                flaks[i].SetActive(true);
            }
            fireLine.SetActive(false);
        }
        if (!other.gameObject.GetComponentInParent<Mob>()) return;
        currentMob = other.gameObject.GetComponentInParent<Mob>();
        
        if (currentMob.hitPoint > 0)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * 30f);
            currentMob.TakeDamage(currentMob.hitPoint + 1);
           // GameEvents.killBySkill.Invoke();
        }
    }
}
