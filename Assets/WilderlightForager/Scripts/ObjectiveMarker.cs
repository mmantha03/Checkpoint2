using UnityEngine;

namespace WilderlightForager
{
    public class ObjectiveMarker : MonoBehaviour
    {
        [SerializeField] private float bobHeight = 0.25f;
        [SerializeField] private float bobSpeed = 2.2f;
        [SerializeField] private float spinSpeed = 55f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = startPosition + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        }
    }
}

