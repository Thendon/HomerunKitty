using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
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

    void Update()
    {
        if (transform.position.y <= -100f)
            Destroy(this.gameObject);
    }
}
