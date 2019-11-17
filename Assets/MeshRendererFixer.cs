using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererFixer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshFilter>())
        {
            MeshFilter renderer = GetComponent<MeshFilter>();
            Mesh slicedMesh = renderer.sharedMesh;

            slicedMesh.RecalculateNormals();
            slicedMesh.RecalculateTangents();
        }
      

    }
    
    
}
