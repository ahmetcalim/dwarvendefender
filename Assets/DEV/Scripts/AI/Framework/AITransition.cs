using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="AI/Transition")]
public class AITransition : ScriptableObject {

    public AICondition[] Conditions;
    public float[] Weights;
    public AIState[] States;
    public int[] Targets;
}
