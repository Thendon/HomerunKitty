using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

// Boids behaviour based on https://github.com/SebLague/Boids
public class BoidController : MonoBehaviour
{
    // Settings

    // Revive hit once based on timer

    // Rule Weights

    public float alignWeight = 1.0f; 
    public float cohesionWeight = 1.0f; 
    public float seperateWeight = 1.0f; 
    public float obstacleAvoidanceWeight = 10.0f;
    public float edgeAvoidanceWeight = 10.0f;

    // --

    public float maxSteerForce = 50;

    public float collisionRadius = 1;

    public float minSpeed = 2;

    public float collisionAvoidanceDistance = 5;

    public float edgeDetectionDistance = 5;

    public float maxSpeed = 50;

    public float perceptionRange = 2.5f;

    public float otherBoidsAvoidanceRange = 1;

    public GameObject boidPrefab;

    public int initialBoidAmount;

    public int boidsPerSecond;

    public Collider spawnArea;

    public ComputeShader boidsComputeShader;

    public Transform boidsParent;

    public BoidData[] boidsData;
    public List<Boid> boids = new List<Boid>();

    const int threadGroupSize = 1024;

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCenter;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }

    public void AddBoid(Boid boid)
    {
        Array.Resize(ref boidsData, boidsData.Length + 1);

        boids.Add(boid);

        boid.transform.SetParent(boidsParent);
    }

    public void RemoveBoid(Boid boid)
    {
        if(boids.Contains(boid))
        {
            int index = boids.IndexOf(boid);

            for (int i = index; i < boids.Count - 2; i++)
            {
                boidsData[i] = boidsData[i + 1];
            }

            Array.Resize(ref boidsData, boidsData.Length - 1);

            boids.RemoveAt(index);
        }
    }

    private void Start()
    {
        boidsData = new BoidData[initialBoidAmount];

        for (int i = 0; i < initialBoidAmount; i++)
        {
            Boid boidInstance = Instantiate(boidPrefab).GetComponent<Boid>();

            BoidData boidData = new BoidData();

            Vector3 spawnPosition = spawnArea.bounds.min;
            spawnPosition.x += (spawnArea.bounds.max.x - spawnArea.bounds.min.x) * UnityEngine.Random.Range(0.0f, 1.0f);
            spawnPosition.z += (spawnArea.bounds.max.z - spawnArea.bounds.min.z) * UnityEngine.Random.Range(0.0f, 1.0f);

            spawnPosition.y = 0.0f;

            boidInstance.transform.position = spawnPosition;

            boidData.position = spawnPosition;

            Vector3 direction = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f) * Vector3.forward;

            boidInstance.transform.LookAt(spawnPosition + direction, Vector3.up);

            boidData.direction = direction;

            boidInstance.rigidBody.velocity = maxSpeed * boidInstance.transform.forward;

            boids.Add(boidInstance);
            boidsData[i] = boidData;

            boidInstance.transform.SetParent(boidsParent);
        }
    }

    private void Update()
    {
        if(boids.Count == 0)
        {
            return;
        }

        // Grab data
        for (int i = 0; i < boids.Count; i++)
        {
            Boid boid = boids[i];
            BoidData data = boidsData[i];

            data.position = boid.transform.position;
            data.direction = boid.transform.forward;
        }

        ComputeBuffer boidBuffer = new ComputeBuffer(boidsData.Length, BoidData.Size);
        boidBuffer.SetData(boidsData);

        boidsComputeShader.SetBuffer(0, "boids", boidBuffer);
        boidsComputeShader.SetInt("numBoids", boidsData.Length);
        boidsComputeShader.SetFloat("viewRadius", perceptionRange);
        boidsComputeShader.SetFloat("avoidRadius", otherBoidsAvoidanceRange);

        int threadGroups = Mathf.CeilToInt(boidsData.Length / (float)threadGroupSize);
        boidsComputeShader.Dispatch(0, threadGroups, 1, 1);

        boidBuffer.GetData(boidsData);

        for (int boidIndex = 0; boidIndex < boids.Count; boidIndex++)
        {
            Boid boid = boids[boidIndex];
            BoidData data = boidsData[boidIndex];

            if (boid.isHit)
            {
                continue;
            }

            data.flockCenter /= data.numFlockmates;

            Vector3 acceleration = Vector3.zero;

            // Flocking behaviour
            if (data.numFlockmates != 0)
            {
                Vector3 offsetToFlockmatesCenter = (data.flockCenter - boid.transform.position);

                var alignmentForce = SteerTowards(data.flockHeading, boid.rigidBody.velocity) * alignWeight;
                var cohesionForce = SteerTowards(offsetToFlockmatesCenter, boid.rigidBody.velocity) * cohesionWeight;
                var seperationForce = SteerTowards(data.avoidanceHeading, boid.rigidBody.velocity) * seperateWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += seperationForce;
            }

            // Obstacle avoidance
            if (Physics.SphereCast(boid.transform.position, collisionRadius, boid.transform.forward, out _, collisionAvoidanceDistance, LayerMask.NameToLayer("Obstacle")))
            {
                Vector3 freeDirection = Vector3.zero;

                int numDirections = 50;
                Vector3[] directions = new Vector3[numDirections];

                float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
                float angleIncrement = Mathf.PI * 2 * goldenRatio;

                for (int i = 0; i < numDirections; i++)
                {
                    float t = (float)i / numDirections;
                    float inclination = Mathf.Acos(1 - 2 * t);
                    float azimuth = angleIncrement * i;

                    float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
                    float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
                    float z = Mathf.Cos(inclination);
                    directions[i] = new Vector3(x, y, z);

                    Vector3 worldDir = boid.transform.TransformDirection(directions[i]);
                    if (!Physics.SphereCast(boid.transform.position, collisionRadius, worldDir, out _, collisionAvoidanceDistance, LayerMask.NameToLayer("Obstacle")))
                    {
                        freeDirection = worldDir;

                        break;
                    }
                }

                Vector3 collisionAvoidForce = SteerTowards(freeDirection, boid.rigidBody.velocity) * obstacleAvoidanceWeight;
                acceleration += collisionAvoidForce;
            }

            // Edge avoidance
            //if (!Physics.Raycast(boid.transform.position + boid.transform.forward * edgeDetectionDistance + Vector3.up * 1000.0f, Vector3.down, out _, float.MaxValue, LayerMask.NameToLayer("Ground")))
            //{
            //    Vector3 freeDirection = Vector3.zero;

            //    int numDirections = 50;
            //    Vector3[] directions = new Vector3[numDirections];

            //    float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
            //    float angleIncrement = Mathf.PI * 2 * goldenRatio;

            //    for (int i = 0; i < numDirections; i++)
            //    {
            //        float t = (float)i / numDirections;
            //        float inclination = Mathf.Acos(1 - 2 * t);
            //        float azimuth = angleIncrement * i;

            //        float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            //        float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            //        float z = Mathf.Cos(inclination);
            //        directions[i] = new Vector3(x, y, z);

            //        Vector3 worldDir = boid.transform.TransformDirection(directions[i]);
            //        if (Physics.Raycast(boid.transform.position + worldDir * edgeDetectionDistance + Vector3.up * 1000.0f, Vector3.down, out _, float.MaxValue, LayerMask.NameToLayer("Ground")))
            //        {
            //            freeDirection = worldDir;

            //            break;
            //        }
            //    }

            //    Vector3 edgeAvoidForce = SteerTowards(freeDirection, boid.rigidBody.velocity) * edgeAvoidanceWeight;
            //    acceleration += edgeAvoidForce;
            //}

            acceleration.y = 0.0f;

            Vector3 velocity = boid.rigidBody.velocity;
            velocity += acceleration * Time.deltaTime;
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            boid.rigidBody.velocity = dir * speed;

            dir.y = 0.0f;

            boid.transform.LookAt(boid.transform.position + dir, Vector3.up);

            if (boid.transform.position.x < spawnArea.bounds.min.x)
            {
                //boid.transform.Translate(Vector3.right * spawnArea.bounds.extents.x);
                boid.rigidBody.MovePosition(Vector3.right * spawnArea.bounds.extents.x);
            }
            if (boid.transform.position.z < spawnArea.bounds.min.z)
            {
                boid.rigidBody.MovePosition(Vector3.forward * spawnArea.bounds.extents.z);
            }

            if (boid.transform.position.x > spawnArea.bounds.max.x)
            {
                boid.rigidBody.MovePosition(-Vector3.right * spawnArea.bounds.extents.x);
            }
            if (boid.transform.position.z > spawnArea.bounds.max.z)
            {
                boid.rigidBody.MovePosition(-Vector3.forward * spawnArea.bounds.extents.z);
            }

        }

        boidBuffer.Release();
    }

    Vector3 SteerTowards(Vector3 vector, Vector3 velocity)
    {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, maxSteerForce);
    }
}