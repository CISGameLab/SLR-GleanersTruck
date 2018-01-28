using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
	public GameObject leader;
	private Rigidbody rb;
	public bool isPickedUp;
	private float moveSpeed;
	
	public PlayerController player;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();
		moveSpeed = 10.0f;
		isPickedUp = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Truck" && isPickedUp && player.gameLost)
		{
			Destroy(gameObject);
		}
		if(collision.gameObject.tag == "Truck" && !isPickedUp)
		{
			PlayerController truck = collision.gameObject.GetComponent<PlayerController>();
			leader = truck.tail;
			truck.tail = gameObject;
			gameObject.layer = 9;
			StartCoroutine(PickUp(truck));
		}
    }

	private IEnumerator PickUp(PlayerController truck)
	{
		yield return new WaitForSeconds(0.25f);
		
		transform.position = leader.transform.position;

		yield return new WaitForSeconds(0.25f);

		gameObject.tag = "TruckElement";
		gameObject.layer = 8;
		isPickedUp = true;
		yield return null;
	}

	private void FixedUpdate()
	{
		if(isPickedUp && leader == null)
		{
			leader = player.gameObject;
		}
		if(isPickedUp)
		{
			Vector3 target = (transform.position - leader.transform.position).normalized;
			transform.position = Vector3.Lerp(transform.position, leader.transform.position + target * player.distance, Time.fixedDeltaTime * moveSpeed);
		}
	}
}
