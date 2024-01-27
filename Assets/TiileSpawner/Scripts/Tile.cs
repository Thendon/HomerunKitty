using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    const float despawnHeight = -100.0f;

    private void Update()
    {
        if(transform.position.y < despawnHeight)
        {
            Destroy(gameObject);
        }
    }

    public void Drop(float TimeToDrop)
    {
        Invoke("Drop", TimeToDrop);
    }

    protected void Drop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if(rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}