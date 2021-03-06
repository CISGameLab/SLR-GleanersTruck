﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour 
{
	private Camera cam;
	private float changeTime;
	private Color newColor;
	private float colorStart;

	// Use this for initialization
	void Start () 
	{
		cam = GetComponent<Camera>();
		changeTime = 10f;
		colorStart = Time.time - changeTime;
	}

	// Update is called once per frame
	private void FixedUpdate() 
	{
		if(Time.time - colorStart > changeTime)
		{
			colorStart = Time.time;
			newColor = Random.ColorHSV(0.0f, 1.0f, 0.25f, 0.75f, 0.25f, 0.75f);
		}
		cam.backgroundColor = Color.Lerp(cam.backgroundColor, newColor, Time.deltaTime / changeTime);
	}
}
