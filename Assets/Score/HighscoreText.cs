using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    public void SetScore(int score)
    {
        text.text = $"Highscore: {score}";
    }

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }
}
