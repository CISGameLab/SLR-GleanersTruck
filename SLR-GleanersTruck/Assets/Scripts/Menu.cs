using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour 
{
	public PlayerController player;

	public GameObject menu;

	public GameObject howToPlay;
	public GameObject tiltControls;
	public GameObject arrowControls;

	public GameObject currentScore;

	public TMP_Text highScore;
	public TMP_Text previousScore;

	public GameObject highScoreContainer;
	public GameObject previousScoreContainer;

	void Start() 
	{
		menu.SetActive(true);
		howToPlay.SetActive(false);
		tiltControls.SetActive(false);
		arrowControls.SetActive(false);
		currentScore.SetActive(false);
		EnableScores();
		if(Application.platform == RuntimePlatform.Android)
		{
			tiltControls.SetActive(true);
		}
		else
		{
			arrowControls.SetActive(true);
		}
	}

	public void HowToPlay()
	{
		howToPlay.SetActive(true);
		DisableScores();
	}

	public void Back()
	{
		howToPlay.SetActive(false);
		EnableScores();
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Play()
	{
		menu.SetActive(false);
		currentScore.SetActive(true);
		DisableScores();
		player.RestartGame();
	}

	private void DisableScores()
	{
		highScoreContainer.SetActive(false);
		previousScoreContainer.SetActive(false);
	}

	public void EnableScores()
	{
		highScoreContainer.SetActive(true);
		previousScoreContainer.SetActive(true);
	}
}
