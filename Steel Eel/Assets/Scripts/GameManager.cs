﻿using System.Collections;
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
        playerTail = playerPieces[playerPieces.Length-2];
	}

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        FadeController.instance.FadeIn();
    }

    public void EndGame(bool win = false)
    {
        FadeController.instance.FadeOut();
        if (win)
        {
            FadeController.instance.FadeOutCompletedEvent += delegate {
                //display win text
                //display default buttons
                //display winner buttons
            };
        }
        else
        {
            FadeController.instance.FadeOutCompletedEvent += delegate {
                //display loser text
                //display default buttons
                //and loser buttons
            };
        }
    }
}