using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    public void LoadImage(float amount)
    {
        transform.localScale = new Vector3(amount, transform.localScale.y, transform.localScale.z);
    }
}
