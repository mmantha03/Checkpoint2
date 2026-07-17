using UnityEngine;

namespace WilderlightForager
{
    public class SmoothFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float distance = 6.8f;
        [SerializeField] private float focusHeight = 1.55f;
        [SerializeField] private float followSpeed = 12f;
        [SerializeField] private float rotationSpeed = 175f;
        [SerializeField] private float minPitch = 5f;
        [SerializeField] private float maxPitch = 38f;
        [SerializeField] private bool lockCursorOnStart = true;

        private float yaw;
        private float pitch = 14f;

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }

        private void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    target = player.transform;
                }
            }

            yaw = transform.eulerAngles.y;

            if (lockCursorOnStart)
            {
                LockCursor();
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                LockCursor();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                yaw = target.eulerAngles.y;
                pitch = 14f;
            }

            float keyboardOrbit = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                keyboardOrbit -= 1f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                keyboardOrbit += 1f;
            }

            yaw += (Input.GetAxisRaw("Mouse X") + keyboardOrbit) * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxisRaw("Mouse Y") * rotationSpeed * 0.45f * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y * 0.75f, 4.5f, 10f);

            Vector3 focusPoint = target.position + Vector3.up * focusHeight;
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = focusPoint + rotation * new Vector3(0f, 0f, -distance);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(focusPoint - transform.position, Vector3.up);
        }

        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
