using System;
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
    public Action onDestroyUpgrade;
    List<PlayerPointsManager> players = new List<PlayerPointsManager>();

    public void Start()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().text = (bonusSize > 0 ? "+" : "-") + Mathf.Abs(bonusSize) + "\n" +
                                                           (bonusWeight > 0 ? "+" : "-") + Mathf.Abs(bonusWeight) + "\n" +
                                                           (cost <= 0 ? "+" : "-") + Mathf.Abs(cost);
    }

    private void OnDestroy()
    {
        foreach (var p in players)
            p.RemoveAvaliableUpgrade(this);

        players.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerPointsManager pointsmanager = other.attachedRigidbody.GetComponentInChildren<PlayerPointsManager>();
        if (pointsmanager == null)
            return;
        if (players.Contains(pointsmanager))
            return;
        pointsmanager.AddAvaliableUprade(this);
        players.Add(pointsmanager);

        //pointsmanager.OnTriggerUpgradeEnter(this.GetComponent<SphereCollider>());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerPointsManager pointsmanager = other.attachedRigidbody.GetComponentInChildren<PlayerPointsManager>();
        if (pointsmanager == null)
            return;
        pointsmanager.RemoveAvaliableUpgrade(this);
        players.Remove(pointsmanager);
    }
}
