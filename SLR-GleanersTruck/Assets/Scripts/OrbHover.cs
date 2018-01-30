using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHover : MonoBehaviour 
{
	public Transform player;
	public Vector3 offset;
	
	void FixedUpdate() 
	{
		Vector3 target = player.position + (player.right * offset.x) + (player.up * offset.y) + (player.forward * offset.z);
		transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime*5f);
		transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, Time.deltaTime*5f);
	}
}
