using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTileSelector : MonoBehaviour
{
    public void Start()
    {
        Upgrade[] upgrades = GetComponentsInChildren<Upgrade>();

        int ran = Random.Range(0, upgrades.Length);
        int upgradeCount = upgrades.Length;
        for(int i = 0; i< upgradeCount; i++)
        {
            if(i != ran)
            {
                Destroy(upgrades[i].gameObject);
            }
        }
    }
}
