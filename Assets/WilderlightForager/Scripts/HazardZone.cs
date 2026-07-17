using UnityEngine;

namespace WilderlightForager
{
    public class HazardZone : MonoBehaviour
    {
        [SerializeField] private float damagePerHit = 8f;

        private void OnTriggerStay(Collider other)
        {
            PlayerStats stats = other.GetComponentInParent<PlayerStats>();

            if (stats != null)
            {
                stats.TakeDamage(damagePerHit, transform.position);
            }
        }
    }
}
