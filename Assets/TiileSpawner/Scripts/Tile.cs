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
        StartCoroutine(DropCoroutine(TimeToDrop));
    }

    private IEnumerator DropCoroutine(float TimeToDrop)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) 
        {
            Debug.Log("No Rigidbody in Tile");
            yield break; 
        }

        yield return new WaitForSeconds(TimeToDrop);
        
        for (int i = 0; i < 10; i++)
        {
            float randWaitTime = Random.Range(0.02f, 0.03f);

            // Get a random vector3
            float angle = Random.Range(0f, 360f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.right * 0.3f;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = direction;
            yield return new WaitForSeconds(randWaitTime);
            randWaitTime = Random.Range(0.02f, 0.03f);
            rb.isKinematic = true;
            rb.useGravity = false;
            yield return new WaitForSeconds(randWaitTime);
        }

        yield return new WaitForSeconds(1.5f);

        rb.isKinematic = false;
        rb.useGravity = true;


        yield return null;
    }

}
