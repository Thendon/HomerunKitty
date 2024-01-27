using HomerunKitty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public TileSpawner tileSpawner;
    public BoidController mouseController;
    public PlayerController playerController;

    void Start()
    {
        tileSpawner.Spawn();
        playerController.GroundPlayer();
        mouseController.InitialSpawn();
    }
}
