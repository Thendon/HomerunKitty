using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public bool isHit;

    public PlayerPointsManager player;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bat"))
        {
            isHit = true;
            player = collision.transform.GetComponentInParent<PlayerPointsManager>();

        }
    }

    public void Update()
    {
        if(transform.position.y < -10)
        {
            KillMouse();
        }
    }

    public void KillMouse()
    {
        if(isHit && player != null)
        {
            player.AddPoints(1 /* + distance from hit*/);
        }

        Destroy(this.gameObject);
    }
}
