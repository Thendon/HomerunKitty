using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public BoidController boidController;

    public Collider spawnArea;

    public float minSpawnAmount;
    public float maxSpawnAmount;

    public float minSpawnIntervallTime;
    public float maxSpawnIntervallTime;

    private float currentIntervallTime;
    private float currentSpawnAmount;
    private float intervallTimer;

    void Start()
    {
        StartNewCycle();
    }

    private void StartNewCycle()
    {
        currentSpawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);
        currentIntervallTime = Random.Range(minSpawnIntervallTime, maxSpawnIntervallTime);
        intervallTimer = 0.0f;
    }

    private void SpawnCycle()
    {
        for (int i = 0; i < currentSpawnAmount; i++)
        {
            Boid boidInstance = Instantiate(boidController.boidPrefab).GetComponentInChildren<Boid>();

            Vector3 spawnPosition = spawnArea.bounds.min;
            spawnPosition.x += (spawnArea.bounds.max.x - spawnArea.bounds.min.x) * UnityEngine.Random.Range(0.0f, 1.0f);
            spawnPosition.z += (spawnArea.bounds.max.z - spawnArea.bounds.min.z) * UnityEngine.Random.Range(0.0f, 1.0f);

            spawnPosition.y = 0.0f;

            boidInstance.transform.position = spawnPosition;

            Vector3 direction = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f) * Vector3.forward;

            boidInstance.transform.LookAt(spawnPosition + direction, Vector3.up);

            boidInstance.rigidBody.velocity = boidController.maxSpeed * boidInstance.transform.forward;

            boidController.AddBoid(boidInstance);
        }
    }

    void Update()
    {
        intervallTimer += Time.deltaTime;

        if(intervallTimer >= currentIntervallTime)
        {
            SpawnCycle();

            StartNewCycle();
        }
    }
}
