using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    public void SetScore(int playerid, int score)
    {
        text.text = $"Player {playerid}: {score}";
    }

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "";
    }
}
