using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRotate : MonoBehaviour 
{
	private float rotateTime;
	private float startRotate;

	private Quaternion targetRot;

	private void Start()
	{
		rotateTime = 1f;
		startRotate = Time.time - rotateTime;
		targetRot = Random.rotation;
	}

	// Update is called once per frame
	private void FixedUpdate() 
	{
		if(Time.time - startRotate > rotateTime)
		{
			startRotate = Time.time;
			targetRot = Random.rotation;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime / rotateTime);
	}
}
