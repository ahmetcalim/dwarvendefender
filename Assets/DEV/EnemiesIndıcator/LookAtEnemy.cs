using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtEnemy : MonoBehaviour
{
    public Transform target;

    void LookAt()
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
    }

    void Update()
    {
        if (target != null)
            LookAt();
        else
        {
            this.transform.parent.GetComponentInParent<IndicatorPoint>().isFull = false;
            Destroy(this.gameObject);
        }

    }
}
