using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTileSelector : MonoBehaviour
{
    public Upgrade[] upgrades;

    public void Start()
    {
        int ran = Random.Range(0, upgrades.Length);
        for(int i = 1; i<upgrades.Length; i++)
        {
            if(i != ran)
            {
                Destroy(upgrades[i].gameObject);

            }
        }
    }
}
