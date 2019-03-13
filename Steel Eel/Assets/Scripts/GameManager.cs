using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public static GameObject playerHead, playerTail;
	public static GameObject[] playerPieces;

	private void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		playerHead = FindObjectOfType<EelMovement>().gameObject;
		playerPieces = GameObject.FindGameObjectsWithTag("Player");
        playerTail = playerPieces[playerPieces.Length-1];
	}

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        FadeController.instance.FadeIn();
    }

    public void EndGame()
    {
        FadeController.instance.FadeOut();
    }
}