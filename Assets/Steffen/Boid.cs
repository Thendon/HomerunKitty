using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour, IHitable
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public PlayerPointsManager player;
    public bool isHit;

    private float respawnTimer;

    const float respawnTime = 2.0f;
    const float respawnVelocityThreshold = 0.2f;

    public void Hit(PlayerPointsManager player)
    {
        isHit = true;
        respawnTimer = 0.0f;
    }

    public void Update()
    {
        if(transform.position.y < -10)
        {
            KillMouse();
        }

        if (isHit)
        {
            Debug.Log(rigidBody.velocity.magnitude);

            if(rigidBody.velocity.magnitude < respawnVelocityThreshold)
            {
                respawnTimer += Time.deltaTime;

                if(respawnTimer > respawnTime)
                {
                    isHit = false;
                }
            }
            else
            {
                respawnTimer = 0.0f;
            }
        }
    }

    public void KillMouse()
    {
        BoidController.instance.RemoveBoid(this);
    }
}
