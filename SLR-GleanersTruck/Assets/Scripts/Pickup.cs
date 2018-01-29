using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
	public GameObject leader;
	public bool isPickedUp;
	private float moveSpeed;

	public MeshRenderer sphereMesh;
	public MeshRenderer tinMesh;

	public Material spherePick;
	public Material sphereTruck;

	public Material tinPick;
	public Material tinTruck;
	
	private PlayerController player;

	// Use this for initialization
	void Start () 
	{
		moveSpeed = 10.0f;
		isPickedUp = false;
		sphereMesh.material = spherePick;
		tinMesh.material = tinPick;

		player = GameObject.FindWithTag("Truck").GetComponent<PlayerController>();
	}

	public void PickUp()
	{
		StartCoroutine(PickUpRoutine());
	}

	private IEnumerator PickUpRoutine()
	{
		leader = player.tail;
		player.tail = gameObject;
		gameObject.layer = 9;

		yield return new WaitForSeconds(0.25f);


		transform.position = leader.transform.position;
		sphereMesh.material = sphereTruck;
		tinMesh.material = tinTruck;
		
		yield return new WaitForSeconds(0.25f);

		gameObject.tag = "TruckElement";
		gameObject.layer = 8;
		isPickedUp = true;
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

	private void OnDestroy() 
	{
        StopAllCoroutines();
    }
}
