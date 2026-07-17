using UnityEngine;

namespace WilderlightForager
{
    public class HealingShrineInteraction : InteractableObject
    {
        [SerializeField] private float healAmount = 45f;

        private bool used;

        public override void Interact(GameObject player)
        {
            if (used)
            {
                WilderlightHUD.Instance?.SetObjective("The healing shrine has already been used.");
                WilderlightAudio.Instance?.PlayDenied(transform.position);
                return;
            }

            PlayerStats stats = player.GetComponentInParent<PlayerStats>();

            if (stats == null)
            {
                return;
            }

            used = true;
            stats.Heal(healAmount, transform.position);
            WilderlightHUD.Instance?.SetObjective("Health restored. Keep moving toward the campfire.");
            gameObject.SetActive(false);
        }
    }
}
