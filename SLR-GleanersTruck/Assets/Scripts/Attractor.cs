using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour 
{
	private float gravity;

	private void Start()
	{
		gravity = -9.807f;
	}

	public void Attract(Rigidbody body)
	{
		Vector3 gravityUp = (body.transform.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;
		body.AddForce(gravityUp * gravity);
		body.transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.transform.rotation;
	}
}
