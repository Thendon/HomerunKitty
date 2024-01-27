using UnityEngine;
using System;

namespace HomerunKitty
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 60;
        public float blendTreeDamping = 0.1f;
        public Animator animator;

        [System.NonSerialized]
        public Vector3 movementInput = Vector3.zero;

        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            ConfigureRigidbody();
        }

        private void Update()
        {
            movementInput = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) movementInput.z += 1;
            if (Input.GetKey(KeyCode.S)) movementInput.z -= 1;

            if (Input.GetKey(KeyCode.D)) movementInput.x += 1;
            if (Input.GetKey(KeyCode.A)) movementInput.x -= 1;
        }

        private void FixedUpdate()
        {
            Vector3 velocity = Camera.main.transform.InverseTransformDirection(movementInput) * speed;
            velocity.y = rb.velocity.y;
            //velocity =  * velocity;
            rb.velocity = velocity;

            Vector3 rotationDirection = velocity;
            rotationDirection.y = 0.0f;
            if (rotationDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection.normalized, Vector3.up);
                //Quaternion deltaRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime)); // deltaRotation * rb.rotation);
            }

            animator?.SetFloat("Running", movementInput.magnitude, blendTreeDamping, Time.deltaTime);
        }

        private void ConfigureRigidbody()
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
