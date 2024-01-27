using UnityEngine;

public class BaseballBatPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 velocity;
    public float debugHitSpeed = 10;
    public float initialWeight = 10000;
    public float bonusWeight;
    public float bonusSize;
    public Rigidbody owner;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;
        if (collision.rigidbody == owner)
            return;

        collision.rigidbody.AddForce(velocity * (initialWeight + bonusWeight));

    }

    public void AddUpgrade(float bonusSize, float bonusWeight, float bonusSpeed)
    {
        this.bonusSize = bonusSize;
        this.bonusWeight = bonusWeight;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x + bonusSize, transform.localScale.z);
    }
}
