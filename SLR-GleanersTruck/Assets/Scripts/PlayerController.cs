using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Rigidbody rb;
	private float moveSpeed;
	private Vector3 moveDir;
	private float rotDir;
	private float rotSpeed;

	public GameObject tail;

	public bool gameLost;

	public float distance;

	void Start() 
	{
		moveSpeed = 10.0f;
		rb = GetComponent<Rigidbody>();
		tail = gameObject;
		rotDir = 0.0f;
		rotSpeed = 2.5f;
		distance = 2.5f;
	}

	void Update()
	{
		rotDir = Input.GetAxis("Horizontal");
	}
	
	void FixedUpdate() 
	{
		if(!gameLost)
		{
			transform.rotation *= Quaternion.Euler(Vector3.up * rotDir * rotSpeed);
			rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "TruckElement" && !gameLost)
		{
			Debug.Log("game lost.");
			gameLost = true;
			distance = 0.0f;
		}
    }
}
