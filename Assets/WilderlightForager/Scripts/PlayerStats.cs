using UnityEngine;

namespace WilderlightForager
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float damageCooldown = 0.75f;

        private float currentHealth;
        private float nextDamageTime;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount, Vector3 sourcePosition)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
            {
                return;
            }

            if (Time.time < nextDamageTime)
            {
                return;
            }

            nextDamageTime = Time.time + damageCooldown;
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            WilderlightAudio.Instance?.PlayDamage(sourcePosition);

            if (currentHealth <= 0f)
            {
                GameManager.Instance?.Lose("You ran out of health. Press R to restart.");
            }
        }

        public void Heal(float amount, Vector3 sourcePosition)
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            WilderlightAudio.Instance?.PlayHeal(sourcePosition);
        }
    }
}
