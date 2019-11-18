using BzKovSoft.ActiveRagdoll;
using BzKovSoft.CharacterSlicer;
using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Polygon;
using RootMotion.Dynamics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;

public class EnemySlashHandler : BzSliceableCharacterBase, IBzSliceableNoRepeat
{
    public bool slicedOnce;
    public GameObject originalPrefab;
    public float SlashDelay = 1;
    private float _slashDelay = 0;
    private int _sliceId;
    
    public PuppetMaster puppetMaster;
    public Mob mob;

    public void Slice(Plane plane, int sliceId, Action<BzSliceTryResult> callBack)
    {
        if (!slicedOnce)
        {
            slicedOnce = true;
        }
        else
        {
            return;
        }
        if (_slashDelay != 0) return;
        if (sliceId == _sliceId) return;

        _sliceId = sliceId;
     
       
        Slice(plane, callBack);
    }
    protected override BzSliceTryData PrepareData(Plane plane)
    {
        // This is where we should implement whatever data gathering procedures we might need from a cut.
        var collidersArr = GetComponentsInChildren<Collider>();
        var ComponentManager = new CharacterComponentManagerFast(gameObject, plane, collidersArr);
        return new BzSliceTryData()
        {
            componentManager = ComponentManager,
            plane = plane,
            addData = null // CHANGE THIS IF YOU WANT TO PASS DATA.
        };
        // throw new System.NotImplementedException();
    }
   
    protected override void OnSliceFinished(BzSliceTryResult result)
    {
        // You can use result.addData to get passed data.
        Debug.Log("A slice is finished.");
        if (result.sliced)
        {
            
            Debug.Log("Turns out, we did slice the target.");
            var neg = result.outObjectNeg;
            var pos = result.outObjectPos;
            if (neg.GetComponentInChildren<Rigidbody>())
            {
                neg.GetComponentInChildren<Rigidbody>().AddForce(Vector3.left * 10f);
            }
            if (pos.GetComponentInChildren<Rigidbody>())
            {
                pos.GetComponentInChildren<Rigidbody>().AddForce(Vector3.right * 10f);
            }
          

        }
    }
    
}
