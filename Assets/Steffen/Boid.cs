using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Boid : MonoBehaviour, IHitable
{
    public Rigidbody rigidBody;

    public Collider boidCollider;

    public PlayerPointsManager player;
    public bool isHit;
    public float hitForce;

    private float respawnTimer;

    const float respawnTime = 2.0f;
    const float respawnVelocityThreshold = 0.2f;

    public List<Vector3> startPositions = new List<Vector3>();
    public List<Quaternion> startRotations = new List<Quaternion>();

    private void Start()
    {
        foreach (Transform child in rigidBody.transform)
        {
            startPositions.Add(child.transform.localPosition);
            startRotations.Add(child.transform.localRotation);
        }
    }

    public void Hit(PlayerPointsManager player, Vector3 force)
    {
        hitForce = force.magnitude;

        if (isHit)
        {
            return;
        }

        this.player = player;
        isHit = true;
        
        respawnTimer = 0.0f;

        rigidBody.AddForce(force);

        foreach (Transform child in rigidBody.transform)
        {
            SphereCollider sphereCollider = child.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.0025f;
            //sphereCollider.radius = 0.0025f;
            Rigidbody boneRigidBody = child.AddComponent<Rigidbody>();
            boneRigidBody.freezeRotation = true;
            boneRigidBody.velocity = rigidBody.velocity;
            boneRigidBody.AddForce(force * 20.0f, ForceMode.Acceleration);
            //boneRigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            SpringJoint joint = rigidBody.transform.AddComponent<SpringJoint>();
            joint.spring = 1000.0f;
            joint.connectedBody = boneRigidBody;
        }
    }

    public void Update()
    {
        if(transform.position.y < -10)
        {
            KillMouse();
        }

        if (isHit)
        {
            if(rigidBody.velocity.magnitude < respawnVelocityThreshold)
            {
                respawnTimer += Time.deltaTime;

                if(respawnTimer > respawnTime)
                {
                    isHit = false;

                    int counter = 0;
                    foreach (Transform child in rigidBody.transform)
                    {
                        Destroy(child.GetComponent<SphereCollider>());
                        Destroy(child.GetComponent<Rigidbody>());

                        child.transform.localPosition = startPositions[counter];
                        child.transform.localRotation = startRotations[counter];

                        counter++;
                    }

                    var springJoints = rigidBody.GetComponents<SpringJoint>();
                    foreach (var springJoint in springJoints)
                    {
                        Destroy(springJoint);
                    }
                }
            }
            else
            {
                respawnTimer = 0.0f;
            }
        }
    }

    public void KillMouse()
    {
        BoidController.instance.RemoveBoid(this);
    }
}
