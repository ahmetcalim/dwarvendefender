using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private bool _struck = false;
    public float DecayTime = 10;
    private bool active = true;
    private bool hit;
    public Transform targetT;
    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(-(transform.position - targetT.position));
    }
    private void OnTriggerEnter(Collider collision)
    {
       

            if (targetT.GetComponent<Spire>())
            {

                FindObjectOfType<Spire>().TakeDamage(1f);
            }
            else
        {
            if (collision.gameObject.layer == 25)
            {


                Camera.main.transform.GetComponent<PlayerHealthTracker>().TakeDamage(1f);
                transform.SetParent(collision.gameObject.transform);
                GetComponent<Rigidbody>().isKinematic = true;
            }
          
           
            }
        Destroy(gameObject, Random.Range(2f,3f));
        
    }
    
}
