﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTextAnimator : MonoBehaviour {

	bool TextActive = false;
	float Speed;
	public Vector3 StartingPos;
	static Color TextColor;
	Vector3 StartingSize = new Vector3(0.13f, 0.13f, 0.13f);
	Vector3 EndingSize = new Vector3(0.16f, 0.16f, 0.16f);
	TextMesh UIText;
	float FadeAmount = 1;
	float TextScaleStart;
	float TextScaleEnd;
	public int AnimateTextType = 0; 

	void Start (){
		transform.localScale = StartingSize;
		StartingPos = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z+0.55f);

		UIText = GetComponent<TextMesh>();
		TextColor = UIText.color;

		TextScaleStart = 0;
		TextScaleEnd = 0;
		Speed = 0;
		FadeAmount = 1;
	}

	void Update (){
		if (TextActive){
			Speed += Time.deltaTime;

			if (AnimateTextType == 1){
				transform.localPosition = new Vector3(StartingPos.x,StartingPos.y+Speed/2,StartingPos.z);
			}

			if (Speed <= 0.4f){
				TextScaleStart += Time.deltaTime * 10;
				transform.localScale = Vector3.Lerp(StartingSize, EndingSize, TextScaleStart);
			}
			if (Speed > 0.4f){
				TextScaleEnd += Time.deltaTime * 15;
				transform.localScale = Vector3.Lerp(EndingSize, StartingSize, TextScaleEnd);

				FadeAmount -= Time.deltaTime;
				Color TempColor = UIText.color;
				TempColor.a = FadeAmount;
				UIText.color = TempColor;
			}

			if (Speed > 1.5f){
				gameObject.SetActive(false);
			}
		}
	}

	void OnEnable (){
		if (AnimateTextType == 2){
			Vector2 RandomPos = new Vector2(StartingPos.x, StartingPos.y)+Random.insideUnitCircle*0.5f;
			if (RandomPos.y < StartingPos.y){
				RandomPos.y = StartingPos.y;
			}
			transform.localPosition = new Vector2(Random.Range(-RandomPos.x,RandomPos.x), RandomPos.y);
		}

		TextActive = true;
	}

	void OnDisable (){
		if (UIText != null){
			TextActive = false;
			TextScaleStart = 0;
			TextScaleEnd = 0;
			Speed = 0;
			FadeAmount = 1;
			UIText.color = TextColor;
			transform.localScale = StartingSize;
			transform.localPosition = StartingPos;
		}
	}
}
