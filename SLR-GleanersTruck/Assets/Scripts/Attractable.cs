using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour 
{
	private Attractor attractor;
	private Rigidbody rb;

	private void Start() 
	{
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = false;
		attractor = GameObject.FindWithTag("World").GetComponent<Attractor>();
	}
	
	private void FixedUpdate() 
	{
		attractor.Attract(rb);
	}
}
