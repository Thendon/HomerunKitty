using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string upgradeName;
    public float bonusSpeed;
    public float bonusWeight;
    public float bonusSize;
    public int cost;

    public void Start()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().text = "Size: +" + bonusSize + "\nWeight: +" + bonusWeight + "\nCost: -" + cost; ;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            if (other.GetComponentInChildren<PlayerPointsManager>() != null)
            {
                PlayerPointsManager pointsmanager = other.GetComponentInChildren<PlayerPointsManager>();
                pointsmanager.OnTriggerUpgradeEnter(this.GetComponent<SphereCollider>());

            }
        }
    }

}
