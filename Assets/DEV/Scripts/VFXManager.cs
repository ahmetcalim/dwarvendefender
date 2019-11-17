using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public Transform vfxTransform;
    public void ActivateVFX(GameObject vfxPrefab)
    {
        GameObject copy = Instantiate(vfxPrefab, new Vector3(vfxTransform.position.x, 0f, vfxTransform.position.z) , Quaternion.Euler(0f, -45f, 0f));
        Destroy(copy, 4f);
    }
    
}
