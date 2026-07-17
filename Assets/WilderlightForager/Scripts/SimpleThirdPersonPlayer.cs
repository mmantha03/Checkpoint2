using UnityEngine;

namespace WilderlightForager
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleThirdPersonPlayer : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float jumpHeight = 1.4f;
        [SerializeField] private float gravity = -24f;
        [SerializeField] private Transform cameraTransform;

        private CharacterController controller;
        private float verticalVelocity;
        private float nextFootstepTime;
        private bool wasGrounded = true;

        public void SetCameraTransform(Transform cameraTarget)
        {
            cameraTransform = cameraTarget;
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
            {
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 input = new Vector3(horizontal, 0f, vertical);
            input = Vector3.ClampMagnitude(input, 1f);

            Vector3 move = GetCameraRelativeMove(input);

            if (move.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 14f * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            verticalVelocity += gravity * Time.deltaTime;

            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            Vector3 velocity = (move * speed) + (Vector3.up * verticalVelocity);
            bool jumped = false;

            if (controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                jumped = true;
            }

            controller.Move(velocity * Time.deltaTime);

            if (jumped)
            {
                WilderlightAudio.Instance?.PlayJump(transform.position);
            }

            if (!wasGrounded && controller.isGrounded)
            {
                WilderlightAudio.Instance?.PlayLand(transform.position);
            }

            if (controller.isGrounded && move.sqrMagnitude > 0.05f && Time.time >= nextFootstepTime)
            {
                WilderlightAudio.Instance?.PlayFootstep(transform.position, isSprinting);
                nextFootstepTime = Time.time + (isSprinting ? 0.28f : 0.42f);
            }

            wasGrounded = controller.isGrounded;
        }

        private Vector3 GetCameraRelativeMove(Vector3 input)
        {
            if (cameraTransform == null)
            {
                return input;
            }

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            return (forward * input.z) + (right * input.x);
        }
    }
}
