using UnityEngine;

public class BaseballBatPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 velocity;
    public float debugHitSpeed = 10;
    public float initialWeight = 10000;
    public float bonusWeight;
    public float bonusSize;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {       
        collision.rigidbody.AddForce(velocity * (initialWeight + bonusWeight));

    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            velocity = new Vector3(debugHitSpeed * Time.deltaTime, 0, 0);
            transform.position += velocity;
        }
    }

    public void AddUpgrade(float bonusSize, float bonusWeight, float bonusSpeed)
    {
        this.bonusSize = bonusSize;
        this.bonusWeight = bonusWeight;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x + bonusSize, transform.localScale.z);
    }
}
