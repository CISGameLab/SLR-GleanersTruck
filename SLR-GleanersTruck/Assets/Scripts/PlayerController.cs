using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour 
{
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

	public GameObject menuObj;
	public Menu menu;

	private AudioSource[] sounds;

	private void Start() 
	{
		sounds = GetComponents<AudioSource>();
		highScore = PlayerPrefs.GetInt("score");
		menu.highScore.text = "High Score: " + highScore.ToString();
		moveSpeed = 10.0f;
		rotDir = 0.0f;
		rotSpeed = 2.5f;
		gameLost = true;
		vcam1.enabled = false;
		vcam2.enabled = true;
		sounds[2].Play();//intro music
		sounds[4].Play();//car idle
	}

	private void Update()
	{
		rotDir = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		if(Input.touchCount > 0 && rotDir == 0.0f)
		{
			Touch touch = Input.GetTouch(0);
			if(touch.position.x < Screen.width / 2)
			{
				rotDir = -1;
			}
			else
			{
				rotDir = 1;
			}
		}
	}
	
	private void FixedUpdate() 
	{
		if(!gameLost)
		{
			transform.rotation *= Quaternion.Euler(Vector3.up * rotDir * rotSpeed);
			Vector3 vel = Vector3.forward * moveSpeed * Time.deltaTime;
			transform.Translate(vel);
		}
	}

	public void RestartGame()
	{
		sounds[6].Play();//car start
		sounds[2].Stop();//menu music
		sounds[1].Play();//game music
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
		sounds[3].Play();//car drive
		SpawnPickup();
		yield return new WaitForSeconds(1f);
		SpawnPickup();
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
		sounds[0].Play();//collect pickups
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
		sounds[8].Play();//pick up
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
		sounds[7].Play();//crash
		sounds[3].Stop();//stop driving
		sounds[1].Stop();//game music
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
		sounds[2].Play();//menu music
		StopAllCoroutines();
		if(PlayerPrefs.HasKey("score"))
		{
			if(score > PlayerPrefs.GetInt("score"))
			{
				SetHighScore();
				sounds[5].Play();//high score
				//display high score UI
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
	}

	public void ButtonNoise()
	{
		sounds[9].Play();//button noise
	}
}
