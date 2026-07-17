using UnityEngine;

namespace WilderlightForager
{
    public class CampfireInteraction : InteractableObject
    {
        [SerializeField] private GameObject litFireVisual;
        [SerializeField] private GameObject objectiveMarker;
        [SerializeField] private GameObject returnBeacon;
        [SerializeField] private Light fireLight;
        [SerializeField] private string completedObjective = "Campfire lit. Follow the green marker to the exit gate.";

        private bool isLit;

        public void Configure(GameObject fireVisual, Light light, GameObject marker, GameObject beacon)
        {
            litFireVisual = fireVisual;
            fireLight = light;
            objectiveMarker = marker;
            returnBeacon = beacon;
        }

        private void Start()
        {
            if (litFireVisual != null)
            {
                litFireVisual.SetActive(false);
            }

            if (fireLight != null)
            {
                fireLight.enabled = false;
            }

            if (returnBeacon != null)
            {
                returnBeacon.SetActive(false);
            }
        }

        public void ShowReturnBeacon()
        {
            if (isLit)
            {
                return;
            }

            if (returnBeacon != null)
            {
                returnBeacon.SetActive(true);
            }
        }

        public override void Interact(GameObject player)
        {
            if (isLit)
            {
                return;
            }

            PlayerInventory inventory = player.GetComponentInParent<PlayerInventory>();

            if (inventory == null)
            {
                return;
            }

            if (!inventory.HasEnoughSuppliesForCampfire())
            {
                WilderlightHUD.Instance?.SetObjective($"Need {inventory.SuppliesStillNeeded()} more supplies before lighting the campfire.");
                WilderlightAudio.Instance?.PlayDenied(transform.position);
                return;
            }

            isLit = true;

            if (litFireVisual != null)
            {
                litFireVisual.SetActive(true);
            }

            if (fireLight != null)
            {
                fireLight.enabled = true;
            }

            if (objectiveMarker != null)
            {
                objectiveMarker.SetActive(false);
            }

            if (returnBeacon != null)
            {
                returnBeacon.SetActive(false);
            }

            WilderlightHUD.Instance?.SetObjective(completedObjective);
            WilderlightHUD.Instance?.HidePrompt();
            WilderlightAudio.Instance?.PlayCampfire(transform.position);
            GameManager.Instance?.RegisterCampfireLit(transform.position);
        }
    }
}
