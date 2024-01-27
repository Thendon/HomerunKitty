using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour, IHitable
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public bool isHit;

    public void Hit()
    {
        isHit = true;
    }
}
