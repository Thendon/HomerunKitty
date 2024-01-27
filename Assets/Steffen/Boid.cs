using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour, IHitable
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public PlayerPointsManager player;
    public bool isHit;

    public void Hit(PlayerPointsManager player)
    {
        isHit = true;
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

        BoidController.instance.RemoveBoid(this);
        //Destroy(this.gameObject);
    }
}
