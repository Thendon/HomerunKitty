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

}
