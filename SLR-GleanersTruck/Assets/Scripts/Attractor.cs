using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour 
{
	private float gravity;
	private float speed;

	private void Start()
	{
		gravity = -9.807f;
		speed = 25.0f;
	}

	public void Attract(Rigidbody body)
	{
		Vector3 gravityUp = (body.transform.position - transform.position).normalized;
		Vector3 bodyUp = body.transform.up;

		body.AddForce(gravityUp * gravity);

		Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.transform.rotation;
		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, Time.fixedDeltaTime * speed);
	}
}
