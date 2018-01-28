using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour 
{
	public Attractor attractor;
	private Rigidbody rb;

	private void Start() 
	{
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = false;
	}
	
	private void Update() 
	{
		attractor.Attract(rb);
	}
}
