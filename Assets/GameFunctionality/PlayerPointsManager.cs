using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPointsManager : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public int points = 0;
    public float bonusWeight;
    public float bonusSpeed;
    public float bonusSize;
    public BaseballBatPhysics bat;
    public LayerMask upgradeLayer;
    public string player;
    public bool dead = false;
    public GameObject endScreen;
    public GameObject scorePrefab;
    public TMP_Text scoreText;

    void Start()
    {
        upgrades = new List<Upgrade>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            BuyUpgrade();
        }
        if (transform.position.y < -10.0f && dead == false)
        {
            dead = true;
            DeadHandler();
        }
    }

    public void BuyUpgrade()
    {
        Debug.Log("UpgradeBuy");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, upgradeLayer))
        {
            Debug.Log("Raycast true");
            Upgrade upgrade = hit.transform.GetComponent<Upgrade>();

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
            hit.transform.gameObject.SetActive(false);
        }
        Debug.Log(hit);

    }

    public void AddPoints(int amount)
    {
        points += amount;

        scoreText.text = "Score: " + points;
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
