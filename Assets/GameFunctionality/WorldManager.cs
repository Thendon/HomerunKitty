using HomerunKitty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public TileSpawner tileSpawner;
    public BoidController mouseController;

    void Start()
    {
        tileSpawner.Spawn();
        mouseController.InitialSpawn();
    }
}
