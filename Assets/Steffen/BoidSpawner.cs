using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public BoidController boidController;

    public Collider spawnArea;

    public float minSpawnAmount = 5;
    public float maxSpawnAmount = 10;

    public float minSpawnIntervallTime = 5;
    public float maxSpawnIntervallTime = 10;

    private float currentIntervallTime;
    private float currentSpawnAmount;
    private float intervallTimer;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        // TODO(Steffen): Dont
        boidController = FindObjectOfType<BoidController>();
    }

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
            if(boidController.boids.Count >= boidController.maxBoidsCount)
            {
                continue;
            }

            Boid boidInstance = Instantiate(boidController.boidPrefab, boidController.boidsParent).GetComponentInChildren<Boid>();

            Vector3 spawnPosition = spawnArea.bounds.min;
            spawnPosition.x += (spawnArea.bounds.max.x - spawnArea.bounds.min.x) * UnityEngine.Random.Range(0.0f, 1.0f);
            spawnPosition.z += (spawnArea.bounds.max.z - spawnArea.bounds.min.z) * UnityEngine.Random.Range(0.0f, 1.0f);

            Physics.Raycast(spawnPosition + Vector3.up * 1000.0f, Vector3.down, out RaycastHit hit, float.MaxValue);
            spawnPosition.y = hit.point.y + boidInstance.boidCollider.bounds.extents.y;

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
