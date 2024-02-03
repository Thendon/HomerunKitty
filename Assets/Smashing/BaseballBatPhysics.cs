using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BaseballBatPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public float debugHitSpeed = 10;
    public float initialWeight = 10000;
    public float bonusWeight;
    public float bonusSize;
    public Rigidbody owner;
    public PlayerPointsManager player;
    public float hitCooldown = 1.0f;
    public GameObject hitEffectPrefab;
    public Quaternion velocityOffset;
    public float velocityLerpFactor = 1.0f;
    public float weightingFactor = 0.2f;
    public Vector3 additionalHitUpforce = Vector3.up;

    Vector3 prevPos;
    Vector3 velocity;

    Dictionary<int, float> hitableNextHitTime = new Dictionary<int, float>();

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        prevPos = rb.position;
    }

    private void FixedUpdate()
    {
        velocity = Vector3.Lerp(velocity, rb.position - prevPos, velocityLerpFactor * Time.fixedDeltaTime);
        prevPos = rb.position;

        lastHitPos = prevPos;
        lastHitForce = velocity;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;
        if (collision.rigidbody == owner)
            return;

        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        int count = collision.GetContacts(contacts);
        float highestContact = 0.3f;
        Vector3 hitPos = collision.gameObject.transform.position + Vector3.up * 1.0f;
        for (int i = 0; i < count; i++)
        {
            Vector3 localPos = transform.InverseTransformPoint(contacts[i].point);
            if (localPos.y > highestContact)
            {
                highestContact = localPos.y;
                hitPos = contacts[i].point;
            }
        }

        Vector3 force = weightingFactor*highestContact * (initialWeight + bonusWeight) * (velocity + Vector3.up * velocity.magnitude);

        IHitable hitable = collision.rigidbody.gameObject.GetComponent<IHitable>();
        if (hitable != null)
        {
            int hash = hitable.GetHashCode();
            if (!hitableNextHitTime.ContainsKey(hash))
                hitableNextHitTime.Add(hash, Time.time + hitCooldown);
            else if (Time.time < hitableNextHitTime[hash])
                return;
            hitableNextHitTime[hash] = Time.time + hitCooldown;


            Vector3 modifiedForce = force + additionalHitUpforce;
            hitable.Hit(player, modifiedForce);

            //VisualEffect hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity).;

            Vector3 hitVelocity = Vector3.ClampMagnitude(force, 5.0f);

            //Debug.Log(force + " -> " + hitVelocity);

            hitEffectPrefab.GetComponent<VisualEffect>().SetVector3("input-hit-velocity", hitVelocity);
            hitEffectPrefab.GetComponent<VisualEffect>().SetVector3("spawnPosition", collision.contacts[0].point);
            hitEffectPrefab.GetComponent<VisualEffect>().Play();

            player.HitScore(hitPos, force.magnitude);
        }
        
        //Debug.Log($"height: {highestContact} * velocity: {velocity.magnitude} * weight {initialWeight} + bonus {bonusWeight} = {force.magnitude}");
        collision.rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    Vector3 lastHitPos;
    Vector3 lastHitForce;

    private void OnDrawGizmos()
    {
        Vector3 force = (initialWeight + bonusWeight) * (lastHitForce + Vector3.up * lastHitForce.magnitude);
        Vector3 force2 = (initialWeight + bonusWeight) * (lastHitForce);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(lastHitPos, lastHitPos + force);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(lastHitPos, lastHitPos + force2);
    }

    Vector3 RotateTowardsUp(Vector3 start, float angle)
    {
        // if you know start will always be normalized, can skip this step
        start.Normalize();

        Vector3 axis = Vector3.Cross(start, Vector3.up);

        // handle case where start is colinear with up
        if (axis == Vector3.zero) axis = Vector3.right;

        return Quaternion.AngleAxis(angle, axis) * start;
    }

    public void SetUpgrade(float bonusSize, float bonusWeight, float bonusSpeed)
    {
        this.bonusSize = bonusSize;
        this.bonusWeight = bonusWeight;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x + bonusSize, transform.localScale.z);
    }
}
