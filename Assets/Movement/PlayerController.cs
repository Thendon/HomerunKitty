using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace HomerunKitty
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 60;
        public float weaponTargetSpeed = 6;
        public float blendTreeDamping = 0.1f;
        public Animator animator;
        public Transform handTarget;
        public float handTargetHeight = 1.0f;
        public float handTargetMaxDistance = 0.5f;
        bool jumpRequested = false;
        public float jumpForce = 10.0f;
        public float jumpCooldown = 1.0f;
        public float groundDetection = 0.05f;
        public LayerMask groundLayerMask;
        public InputManagerSystem input;
        public bool useNewInput => input != null;

        [System.NonSerialized]
        public Vector3 movementInput = Vector3.zero;
        [NonSerialized]
        public Vector3 aimInput = Vector3.zero;

        float nextJump = 0.0f;
        const float inputEpsilon = 0.01f;

        Rigidbody rb;
        CapsuleCollider capsuleCollider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            capsuleCollider = GetComponentInChildren<CapsuleCollider>();

            ConfigureRigidbody();
        }

        public void GroundPlayer()
        {
            RaycastHit hit;
            
            const float castOriginDistance = 100f;
            const float castDistance = 200f;
            
            Vector3 castOrigin = transform.position + Vector3.up * castOriginDistance;
            float radius = capsuleCollider.radius;

            if (Physics.SphereCast(castOrigin, radius, Vector3.down, out hit, castDistance, groundLayerMask))
            {
                rb.position = hit.point;
                //transform.position = hit.point;
            }
            else
            {
                Assert.IsTrue(false);
                Debug.Break();
            }

            /*
            Ray ray = new Ray(transform.position + Vector3.up * 1000.0f, Vector3.down);
            if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayerMask))
            {
                transform.position = hit.point;
            }
            else
            {
                Assert.IsTrue(false);
                Debug.Break();
            }
            */
        }

        public bool IsTouchingGround()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.01f, -Vector3.up);
            return Physics.Raycast(ray, out RaycastHit hit, groundDetection, groundLayerMask);
        }

        private void Update()
        {
            if (nextJump > 0.0f)
            {
                nextJump -= Time.deltaTime;
            }

            movementInput = Vector3.zero;

            if (useNewInput)
            {
                movementInput = new Vector3(input.move.x, 0.0f, input.move.y);
                movementInput.Normalize();
                if (input.jump && nextJump <= 0.0f && IsTouchingGround())
                    jumpRequested = input.jump;

                if (input.mouseAim)
                    aimInput = GetMouseAim(input.aim);
                else
                    aimInput = new Vector3(input.aim.x, 0.0f, input.aim.y) * handTargetMaxDistance;
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                    movementInput.z += 1;
                if (Input.GetKey(KeyCode.S))
                    movementInput.z -= 1;

                if (Input.GetKey(KeyCode.D))
                    movementInput.x += 1;
                if (Input.GetKey(KeyCode.A))
                    movementInput.x -= 1;

                if (Input.GetKey(KeyCode.Space) && nextJump <= 0.0f && IsTouchingGround())
                {
                    jumpRequested = true;
                    nextJump = jumpCooldown;
                }

                aimInput = GetMouseAim(Input.mousePosition);
            }
        }

        Vector3 GetMouseAim(Vector2 mousePos)
        {
            //when using mouse
            Ray camRay = Camera.main.ScreenPointToRay(mousePos);
            Plane playerPlane = new Plane(Vector3.up, transform.position + handTargetHeight * Vector3.up);
            playerPlane.Raycast(camRay, out float hitDist);
            Vector3 hitPoint = camRay.origin + camRay.direction * hitDist;
            hitPoint.y = transform.position.y;
            return Vector3.ClampMagnitude(hitPoint - transform.position, handTargetMaxDistance);
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

            if (jumpRequested)
            {
                jumpRequested = false;
                rb.AddForce(Vector3.up * jumpForce);
            }

            animator.SetFloat("Running", movementInput.magnitude, blendTreeDamping, Time.deltaTime);

            if (aimInput.sqrMagnitude > inputEpsilon)
            {
                handTarget.position = transform.position + Vector3.up * handTargetHeight + aimInput;
                bool fullyExtended = aimInput.sqrMagnitude >= handTargetMaxDistance * handTargetMaxDistance - 0.01f;
                Vector3 aimDir = aimInput.normalized;
                Vector3 forward = fullyExtended ? aimDir : Vector3.up;
                Vector3 right = Vector3.Dot(Vector3.up, forward) < 0.999f ?
                    Vector3.Cross(Vector3.up, forward) :
                    Vector3.Cross(-aimDir, forward);
                Vector3 up = Vector3.Cross(forward, right);
                    
                Quaternion target = Quaternion.LookRotation(forward, up);
                handTarget.rotation = Quaternion.Lerp(handTarget.rotation, target, Time.deltaTime * weaponTargetSpeed);
            }
        }

        private void ConfigureRigidbody()
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
