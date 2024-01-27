using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    public TMPro.TextMeshProUGUI player1Result;
    public TMPro.TextMeshProUGUI player2Result;

    private PlayerPointsManager[] playerPoints;

    public void Start()
    {
        playerPoints = FindObjectsByType<PlayerPointsManager>(FindObjectsSortMode.InstanceID);
        if (!GameManager.instance.multiplayerActive)
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
        }
        else
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
            this.player2Result.text = "Player 2: " + playerPoints[1].points.ToString();

        }
    }



    public void Exit()
    {
        Application.Quit();
    }


}
