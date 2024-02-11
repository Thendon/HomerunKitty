using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI[] playerResults = new TextMeshProUGUI[0];

    public void Start()
    {
        List<PlayerPointsManager> points = PlayerManager.instance.players;
        List<PlayerPointsManager> sortedPoints = points.OrderByDescending(p => p.points).ToList();

        for (int i = 0; i < playerResults.Length; i++)
        {
            if (i < GameManager.instance.playerCount)
                playerResults[i].text = (i + 1) + ". place is player " + (sortedPoints[i].playerid + 1) + " with " + sortedPoints[i].points.ToString();
            else
                playerResults[i].enabled = false;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("UIScene");
        GameManager.instance.FindNewMenu();
    }

    public void Restart()
    {
        PlayerManager.instance.Reset();

        GameManager.instance.LoadLevel();
    }

    public void BackToMainMenu()
    {
        PlayerManager.instance.Reset();
        GameManager.instance.ingameSong.Stop();
        SceneManager.LoadScene("UIScene");
    }
}
