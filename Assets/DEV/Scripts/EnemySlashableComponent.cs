using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlashableComponent : MonoBehaviour
{
    public EnemySlashHandler SlashHandler;
    // Start is called before the first frame update
    void Start()
    {
        SlashHandler = GetComponentInParent<EnemySlashHandler>();
    }
    
}
