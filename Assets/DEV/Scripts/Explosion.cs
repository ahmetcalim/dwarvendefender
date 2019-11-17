using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("head"))
        {
            if (other.gameObject.GetComponent<Rigidbody>())
            {
                if (other.gameObject.GetComponentInParent<Animator>())
                {
                    other.gameObject.GetComponentInParent<Animator>().enabled = false;
                }

                Vector3 direction = Vector3.Reflect((transform.position - other.contacts[0].point).normalized, other.contacts[0].normal);
                other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 1000f, ForceMode.Impulse);
            }
        }
    }
}
