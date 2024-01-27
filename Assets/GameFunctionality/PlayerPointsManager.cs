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


    void Start()
    {
        upgrades = new List<Upgrade>();
    }

    void Update()
    {
        
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        bonusSize += upgrade.bonusSize;
        bonusWeight += upgrade.bonusWeight;
        bonusSpeed += upgrade.bonusSpeed;

    }

    public void AddPoints(int amount)
    {
        points += amount;
    }
}
