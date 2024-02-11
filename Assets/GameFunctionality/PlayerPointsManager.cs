using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class PlayerPointsManager : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public int points = 0;
    private int highscore;
    public float bonusWeight;
    public float bonusSpeed;
    public float bonusSize;
    public BaseballBatPhysics bat;
    public LayerMask upgradeLayer;
    public string player;
    public bool dead = false;
    public GameObject scorePrefab;
    public ScoreText scoreText;
    public HighscoreText highScoreText;
    private Vector3 startScale;
    public int playerid = 0;
    List<Upgrade> avaliableUpgrades = new List<Upgrade>();

    public AnimationCurve curve;

    void Start()
    {
        highScoreText = FindFirstObjectByType<HighscoreText>();

        upgrades = new List<Upgrade>();

        if (PlayerPrefs.HasKey("Highscore"))
        {
            highscore = PlayerPrefs.GetInt("Highscore");

            highScoreText.SetScore(highscore);
        }
        else
        {
            highscore = 0;
        }

        startScale = scoreText.transform.localScale;
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    BuyUpgrade();
        //}
        if (transform.position.y < -10.0f && dead == false)
        {
            dead = true;
            PlayerManager.instance.HandlePlayerDeath(this);
        }
    }

    Upgrade GetAvaliableUpgrade()
    {
        foreach (Upgrade upgrade in avaliableUpgrades)
        {
            if (points >= upgrade.cost)
                return upgrade;
        }
        return null;
    }

    public void BuyUpgrade()
    {
        Upgrade upgrade = GetAvaliableUpgrade();

        if (upgrade == null)
            return;

        upgrades.Add(upgrade);
        if(bat.transform.localScale.y + upgrade.bonusSize < 0.2)
        {
            bonusSize = -(bat.transform.localScale.y * 0.8f); 
        }
        else
        {
            bonusSize += upgrade.bonusSize;
        }
        if (bat.initialWeight + bat.bonusWeight + upgrade.bonusWeight < 100)
        {
            bonusWeight += -(bat.initialWeight + bat.bonusWeight) * 0.8f;
        }
        else
        {
            bonusWeight +=upgrade.bonusWeight;

        }
        
        bonusSpeed += Mathf.Max(upgrade.bonusSpeed, 0.0f);
        AddPoints(-upgrade.cost);
        bat.SetUpgrade(bonusSize, bonusWeight,bonusSpeed);

        Destroy(upgrade.gameObject);
    }

    public void AddPoints(int amount)
    {
        points += amount;

        scoreText.SetScore(playerid, points);//"Score: " + points;

        if (scoringRoutine != null)
        {
            StopCoroutine(scoringRoutine);
        }

        scoringRoutine = StartCoroutine(ScoringAnim());

        if(points > highscore)
        {
            highscore = points;

            PlayerPrefs.SetInt("Highscore", highscore);

            highScoreText.SetScore(highscore);
        }
    }

    private Coroutine scoringRoutine;

    private IEnumerator ScoringAnim()
    {
        scoreText.transform.localScale = startScale;

        float progress = 0.0f;
        float speed = 1.0f;

        while (progress < curve.keys[curve.keys.Length - 1].time)
        {
            progress += Time.deltaTime * speed;

            scoreText.transform.localScale = startScale + (curve.Evaluate(progress) + 0.5f) * Vector3.one;

            yield return new WaitForEndOfFrame();
        }

        scoringRoutine = null;
    }
    
    public void HitScore(Vector3 hitPos, float amount)
    {
        ScoreEffect scoreInstance = Instantiate(scorePrefab).GetComponent<ScoreEffect>();

        amount = Mathf.Max(1f, amount);

        scoreInstance.Init(amount);
        scoreInstance.transform.position = hitPos;
    }

    public void AddAvaliableUprade(Upgrade upgrade)
    {
        avaliableUpgrades.Add(upgrade);
    }

    public void RemoveAvaliableUpgrade(Upgrade upgrade)
    {
        avaliableUpgrades.Remove(upgrade);
    }
}
