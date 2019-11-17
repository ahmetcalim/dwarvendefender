using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour {

    public AIState ActiveState;
    public AIAction ActiveAction;
    public bool AIActive;
    public float DetectionRange;
    public GameObject[] Targets;
    public int ActiveTarget = 0;

    public float RetargetTime = 10;
    [HideInInspector] public float retarget = 0;

    private void Start()
    {
        Targets = new GameObject[ActiveState.Scanners.Length];
    }

    void Update () {
        if (!AIActive) return;
        retarget += Time.deltaTime;
        
        ActiveState.UpdateState(this);
    }
}
