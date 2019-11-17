using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : MonoBehaviour
{
    private Mob currentMob;
    private Transform spire;
    private void Start()
    {
        spire = FindObjectOfType<Spire>().transform;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.GetComponentInParent<Mob>()) return;

        StartCoroutine(Hit(collision));
    }
    IEnumerator Hit(Collision collision)
    {
        if (collision != null)
        {
            currentMob = collision.transform.GetComponentInParent<Mob>();
            if (currentMob.navMeshAgent.isOnNavMesh)
            {
                currentMob.navMeshAgent.enabled = false;
                currentMob.animator.enabled = false;
            }
            yield return new WaitForSeconds(.1f);
            currentMob.animator.enabled = true;
            currentMob.Stagger();
            yield return new WaitForSeconds(.5f);
            currentMob.navMeshAgent.enabled = true;
            if (currentMob.navMeshAgent.isOnNavMesh)
            {
                currentMob.navMeshAgent.SetDestination(spire.position);
            }
        }
    

    }
}
