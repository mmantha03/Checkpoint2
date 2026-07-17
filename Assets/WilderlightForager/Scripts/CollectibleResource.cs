using UnityEngine;

namespace WilderlightForager
{
    public class CollectibleResource : InteractableObject
    {
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;
        [SerializeField] private int amount = 1;

        public void Configure(ResourceType type, int value)
        {
            resourceType = type;
            amount = value;
        }

        public override void Interact(GameObject player)
        {
            PlayerInventory inventory = player.GetComponentInParent<PlayerInventory>();

            if (inventory == null)
            {
                return;
            }

            inventory.AddResource(resourceType, amount);
            WilderlightAudio.Instance?.PlayPickup(resourceType, transform.position);
            WilderlightHUD.Instance?.HidePrompt();
            Destroy(gameObject);
        }
    }
}
