using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMenuPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public IEnumerator SetKinematicFalse()
    {

        yield return new WaitForSeconds(.5f);
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Chest"))
        {
            GetComponent<A_VR_PositionAdjuster>().colliderUpdatePerFrame = false;
        }
        else
        {
            GetComponent<A_VR_PositionAdjuster>().colliderUpdatePerFrame = true;

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        GetComponent<A_VR_PositionAdjuster>().colliderUpdatePerFrame = true;
    }
}
