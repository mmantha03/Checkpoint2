using UnityEngine;

namespace WilderlightForager
{
    public class FloatingCollectible : MonoBehaviour
    {
        [SerializeField] private float bobHeight = 0.18f;
        [SerializeField] private float bobSpeed = 2f;
        [SerializeField] private float spinSpeed = 65f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            transform.position = startPosition + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        }
    }
}

