﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatText : MonoBehaviour {

	Emerald_AI EmeraldComponent;
	Camera m_Camera;
	Vector3 GlobalScale;

	void Start () {
		EmeraldComponent = transform.parent.GetComponent<Emerald_AI>();
		m_Camera = Camera.main;
		//GlobalScale = transform.root.localScale;
		GlobalScale = EmeraldComponent.transform.localScale;
	}
	
	//Make new script for actual text. When enabled, animate upwards. if disabled, go back to original position
	void Update () {
		if (EmeraldComponent.CurrentDetectionRef == Emerald_AI.CurrentDetection.Alert
			|| EmeraldComponent.BehaviorRef == Emerald_AI.CurrentBehavior.Companion){
			transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
				m_Camera.transform.rotation * Vector3.up);

			float dist = Vector3.Distance(m_Camera.transform.position, transform.position);
			if (dist < 40 && dist > 15){
				transform.localScale = new Vector3(dist*0.085f/GlobalScale.x,dist*0.085f/GlobalScale.y,dist*0.085f/GlobalScale.z);
			}
		}
	}
}
