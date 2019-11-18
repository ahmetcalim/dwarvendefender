using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flickable : MonoBehaviour {

    public Vector3 FlickThreshold;
    public float FlickAngleTolerance;

    public UnityEvent OnFlick;

    public void Flick(Vector3 d)
    {
        //Check flick angle tolerance first
        var tf = transform.TransformVector(FlickThreshold);

        if (Mathf.Abs(Vector3.Angle(d, tf)) > FlickAngleTolerance) return;
        if (d.magnitude < tf.magnitude) return;
        OnFlick.Invoke();
    }
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
