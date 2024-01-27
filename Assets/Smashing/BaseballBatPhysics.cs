using UnityEngine;

public class BaseballBatPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public float debugHitSpeed = 10;
    public float initialWeight = 10000;
    public float bonusWeight;
    public float bonusSize;
    public Rigidbody owner;
    public PlayerPointsManager player;

    Vector3 prevPos;
    Vector3 velocity;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        prevPos = rb.position;
    }

    private void FixedUpdate()
    {
        velocity = rb.position - prevPos;
        prevPos = rb.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;
        if (collision.rigidbody == owner)
            return;

        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        int count = collision.GetContacts(contacts);
        float highestContact = 0.0f;
        for (int i = 0; i < count; i++)
        {
            Vector3 localPos = transform.InverseTransformPoint(contacts[i].point);
            if (localPos.y > highestContact)
                highestContact = localPos.y;
        }

        IHitable hitable = collision.rigidbody.gameObject.GetComponent<IHitable>();
        if (hitable != null)
        {
            hitable.Hit(player);
        }

        Vector3 force = highestContact * velocity * (initialWeight + bonusWeight);
        Debug.Log($"height: {highestContact} * velocity: {velocity.magnitude} * weight {initialWeight} + bonus {bonusWeight} = {force.magnitude}");
        collision.rigidbody.AddForce(force);
    }

    public void AddUpgrade(float bonusSize, float bonusWeight, float bonusSpeed)
    {
        this.bonusSize = bonusSize;
        this.bonusWeight = bonusWeight;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x + bonusSize, transform.localScale.z);
    }
}
