using UnityEngine;

public class BaseballBatPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 velocity;
    public float debugHitSpeed = 10;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {       
        collision.rigidbody.AddForce(velocity * 100000f);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            velocity = new Vector3(debugHitSpeed * Time.deltaTime, 0, 0);
            transform.position += velocity;
        }
    }
}
