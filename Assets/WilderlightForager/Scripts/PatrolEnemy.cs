using UnityEngine;

namespace WilderlightForager
{
    public class PatrolEnemy : MonoBehaviour
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float patrolSpeed = 1.9f;
        [SerializeField] private float chaseSpeed = 3.1f;
        [SerializeField] private float chaseRange = 6.5f;
        [SerializeField] private float damageRange = 1.6f;
        [SerializeField] private float damageAmount = 8f;

        private Transform targetPoint;
        private Transform player;

        public void Configure(Transform startPoint, Transform endPoint)
        {
            pointA = startPoint;
            pointB = endPoint;
            targetPoint = pointB;
        }

        private void Start()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            targetPoint = pointB != null ? pointB : pointA;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
            {
                return;
            }

            Vector3 destination = GetDestination();
            MoveToward(destination);

            if (player != null && Vector3.Distance(transform.position, player.position) <= damageRange)
            {
                PlayerStats stats = player.GetComponent<PlayerStats>();

                if (stats != null)
                {
                    stats.TakeDamage(damageAmount, transform.position);
                }
            }
        }

        private Vector3 GetDestination()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= chaseRange)
            {
                return player.position;
            }

            if (targetPoint == null)
            {
                return transform.position;
            }

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA;
            }

            return targetPoint != null ? targetPoint.position : transform.position;
        }

        private void MoveToward(Vector3 destination)
        {
            destination.y = transform.position.y;
            Vector3 direction = destination - transform.position;

            if (direction.sqrMagnitude < 0.05f)
            {
                return;
            }

            float speed = player != null && Vector3.Distance(transform.position, player.position) <= chaseRange ? chaseSpeed : patrolSpeed;
            Vector3 step = direction.normalized * speed * Time.deltaTime;
            transform.position += step;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 10f * Time.deltaTime);
        }
    }
}
