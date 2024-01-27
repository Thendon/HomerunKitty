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
        public Transform handTarget;
        public float handTargetHeight = 1.0f;
        public float handTargetMaxDistance = 0.5f;

        [System.NonSerialized]
        public Vector3 movementInput = Vector3.zero;
        [NonSerialized]
        public Vector3 aimInput = Vector3.zero;

        const float inputEpsilon = 0.01f;

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

            //when using mouse
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            playerPlane.Raycast(camRay, out float hitDist);
            Vector3 hitPoint = camRay.origin + camRay.direction * hitDist;
            hitPoint.y = transform.position.y;
            aimInput = Vector3.ClampMagnitude(hitPoint - transform.position, handTargetMaxDistance);
        }

        private void FixedUpdate()
        {
            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(Camera.main.transform.rotation * Vector3.forward, Vector3.up).normalized;
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Vector3.up);

            Vector3 velocity = cameraPlanarRotation * movementInput * speed;
            velocity.y = rb.velocity.y;
            //velocity =  * velocity;
            rb.velocity = velocity;

            Vector3 rotationDirection = velocity;
            rotationDirection.y = 0.0f;
            if (rotationDirection.sqrMagnitude > inputEpsilon)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection.normalized, Vector3.up);
                //Quaternion deltaRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime)); // deltaRotation * rb.rotation);
            }

            animator.SetFloat("Running", movementInput.magnitude, blendTreeDamping, Time.deltaTime);

            if (aimInput.sqrMagnitude > inputEpsilon)
            {
                handTarget.position = transform.position + Vector3.up * handTargetHeight + aimInput;
            }
        }

        private void ConfigureRigidbody()
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
