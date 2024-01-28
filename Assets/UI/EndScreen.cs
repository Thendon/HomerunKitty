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
    public TMPro.TextMeshProUGUI player3Result;
    public TMPro.TextMeshProUGUI player4Result;

    private PlayerPointsManager[] playerPoints;

    public void Start()
    {
        playerPoints = FindObjectsByType<PlayerPointsManager>(FindObjectsSortMode.InstanceID);
        if (GameManager.instance.playerCount == 1)
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
        }
        else
        if (GameManager.instance.playerCount == 2)
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
            this.player2Result.text = "Player 2: " + playerPoints[1].points.ToString();

        }
        else
        if (GameManager.instance.playerCount == 3)
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
            this.player2Result.text = "Player 2: " + playerPoints[1].points.ToString();
            this.player3Result.text = "Player 3: " + playerPoints[2].points.ToString();

        }
        else
        if (GameManager.instance.playerCount == 4)
        {
            this.player1Result.text = "Player 1: " + playerPoints[0].points.ToString();
            this.player2Result.text = "Player 2: " + playerPoints[1].points.ToString();    
            this.player3Result.text = "Player 3: " + playerPoints[2].points.ToString();
            this.player4Result.text = "Player 4: " + playerPoints[3].points.ToString();

        }
    }



    public void Exit()
    {
        SceneManager.LoadScene("UIScene");
        GameManager.instance.FindNewMenu();
    }


}
