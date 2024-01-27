using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public bool isHit;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bat"))
        {
            isHit = true;
        }
    }
}
