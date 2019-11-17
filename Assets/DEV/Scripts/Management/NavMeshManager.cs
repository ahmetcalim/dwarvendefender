using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NavMesh.onPreUpdate += NavMeshSettings;
      
    }
    private void NavMeshSettings()
    {
        NavMesh.avoidancePredictionTime = 4;
        NavMesh.pathfindingIterationsPerFrame = 150;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
