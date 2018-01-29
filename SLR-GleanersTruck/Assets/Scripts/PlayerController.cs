using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

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

	public int pickups;

	public int score;
	public int highScore;

	public TMP_Text scoreText;

	public GameObject pickupSpawn;
	public GameObject world;

	public CinemachineVirtualCamera vcam1;
	public CinemachineVirtualCamera vcam2;
	private CinemachineTransposer transposer;
	private float lerpTime;

	public GameObject menuObj;
	public Menu menu;

	private void Start() 
	{
		transposer = vcam1.GetCinemachineComponent<CinemachineTransposer>();
		rb = GetComponent<Rigidbody>();
		moveSpeed = 10.0f;
		rotDir = 0.0f;
		rotSpeed = 2.5f;
		lerpTime = 1f;
		gameLost = true;
		vcam1.enabled = false;
		vcam2.enabled = true;
		//load in high score
	}

	private void Update()
	{
		rotDir = Input.GetAxis("Horizontal");
	}
	
	private void FixedUpdate() 
	{
		if(!gameLost)
		{
			transform.rotation *= Quaternion.Euler(Vector3.up * rotDir * rotSpeed);
			rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
		}
	}

	public void RestartGame()
	{
		vcam1.enabled = true;
		vcam2.enabled = false;
		gameLost = false;
		moveSpeed = 10.0f;
		pickups = 0;
		score = 0;
		scoreText.text = pickups.ToString();
		distance = 2.5f;
		tail = gameObject;
		StartCoroutine(SpawnInitPickups());
	}

	private IEnumerator SpawnInitPickups()
	{
		yield return new WaitForSeconds(1f);
		SpawnPickup();
		yield return new WaitForSeconds(1f);
		SpawnPickup();
		yield return new WaitForSeconds(1f);
		SpawnPickup();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "TruckElement")
		{
			if(gameLost)
			{
				Destroy(collision.gameObject);
				RemovePickup();
			}
			else
			{
				EndGame();
				Destroy(collision.gameObject);
				RemovePickup();
			}
		}
		else if(collision.gameObject.tag == "Pickup")
		{
			AddPickup();
			Pickup pick = collision.gameObject.GetComponent<Pickup>();
			pick.PickUp();
		}
    }

	private void RemovePickup()
	{
		StopAllCoroutines();
		StartCoroutine(Timeout());
		pickups--;
		if(pickups == 0)
		{
			menuObj.SetActive(true);
			SetScore();
		}
		//play noise
	}

	private void AddPickup()
	{
		pickups++;
		score = pickups;
		scoreText.text = score.ToString();
		moveSpeed = moveSpeed * 1.01f;

		SpawnPickup();
	}

	private IEnumerator Timeout()
	{
		yield return new WaitForSeconds(0.5f);
		menuObj.SetActive(true);
		SetScore();
	}

	private void SpawnPickup()
	{
		float radius = world.GetComponent<SphereCollider>().radius;
		Vector3 position = Random.onUnitSphere * 30f;
		Instantiate(pickupSpawn, position, pickupSpawn.transform.rotation);
	}

	private void EndGame()
	{
		vcam1.enabled = false;
		vcam2.enabled = true;
		List<GameObject> pickupsToDispose = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pickup"));
		foreach(GameObject pick in pickupsToDispose)
		{
			Destroy(pick);
		}
		gameLost = true;
		distance = 0.0f;
	}

	private void SetScore()
	{
		StopAllCoroutines();
		if(PlayerPrefs.HasKey("score"))
		{
			if(score > PlayerPrefs.GetInt("score"))
			{
				SetHighScore();
			}
		}
		else
		{
			SetHighScore();
		}
		highScore = PlayerPrefs.GetInt("score");
		menu.highScore.text = "High Score: " + highScore.ToString();
		menu.previousScore.text = "Score: " + score.ToString();
		menu.currentScore.SetActive(false);
		menu.EnableScores();
	}

	private void SetHighScore()
	{
		PlayerPrefs.SetInt("score", score);
		//new high score
	}
}
