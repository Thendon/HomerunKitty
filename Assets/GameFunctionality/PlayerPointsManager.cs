using System.Collections;
using System.Collections.Generic;
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
    }

    public void BuyUpgrade()
    {
        Debug.Log("UpgradeBuy");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 20f, upgradeLayer))
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
                points -= upgrade.cost;
                bat.AddUpgrade(bonusSize, bonusWeight,bonusSpeed);

            }
        }
        Debug.Log(hit);

    }

    public void AddPoints(int amount)
    {
        points += amount;
    }
}
