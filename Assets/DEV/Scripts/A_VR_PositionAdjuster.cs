using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class A_VR_PositionAdjuster : MonoBehaviour
{
    public bool collider;
    public bool pos;
    public bool colliderUpdatePerFrame;
    // Start is called before the first frame update
    void Start()
    {
        if (pos)
        {
            AdjustPosition();
        }
        if (collider)
        {
            StartCoroutine(AfterSeconds());
        }
    }
    private void Update()
    {
        if (colliderUpdatePerFrame)
        {
            AdjustCollider();
        }
    }
    IEnumerator AfterSeconds()
    {
        yield return new WaitForSeconds(1f);
        AdjustCollider();
    }
    public void AdjustPosition()
    {
        transform.localPosition += new Vector3(-Camera.main.transform.localPosition.x, 0f, -Camera.main.transform.localPosition.z);
    }
    public void AdjustCollider()
    {
        GetComponent<CapsuleCollider>().center = new Vector3(Camera.main.transform.localPosition.x, .65f, Camera.main.transform.localPosition.z);
    }
}
