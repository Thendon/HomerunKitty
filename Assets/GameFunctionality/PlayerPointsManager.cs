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
    public GameObject endScreen;
    public GameObject scorePrefab;
    public ScoreText scoreText;
    public HighscoreText highScoreText;
    private Vector3 startScale;
    public int playerid = 0;

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
            DeadHandler();
        }
    }

    public void OnTriggerUpgradeEnter(Collider other)
    {
        Debug.Log("UpgradeBuy");
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, upgradeLayer))
        //if(other.gameObject.layer == upgradeLayer)
        //{
            Debug.Log("Raycast true");
            Upgrade upgrade = other.transform.GetComponent<Upgrade>();

            if (!upgrades.Contains(upgrade) && points >= upgrade.cost)
            {
                Debug.Log("adding Upgrade");
                upgrades.Add(upgrade);
                bonusSize += upgrade.bonusSize;
                bonusWeight += upgrade.bonusWeight;
                bonusSpeed += upgrade.bonusSpeed;
                AddPoints(-upgrade.cost);
                bat.AddUpgrade(bonusSize, bonusWeight,bonusSpeed);

            }
            other.transform.gameObject.SetActive(false);
        //}
        //Debug.Log(collision);

    }

    public void AddPoints(int amount)
    {
        points += amount;

        scoreText.SetScore(playerid, amount);//"Score: " + points;

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


    public void DeadHandler()
    {
        GameObject.Instantiate(endScreen);
    }

    
    public void HitScore(Vector3 hitPos, float amount)
    {
        ScoreEffect scoreInstance = Instantiate(scorePrefab).GetComponent<ScoreEffect>();
        scoreInstance.Init(amount);
        scoreInstance.transform.position = hitPos;
    }
}
