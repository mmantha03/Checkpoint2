using UnityEngine;

namespace WilderlightForager
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int suppliesNeededForCampfire = 6;

        public int Wood { get; private set; }
        public int Food { get; private set; }
        public int Crystal { get; private set; }
        public int TotalSupplies => Wood + Food + Crystal;
        public int SuppliesNeededForCampfire => suppliesNeededForCampfire;

        private bool campfireReady;

        private void Start()
        {
            WilderlightHUD.Instance?.SetObjective("Collect six supplies, then return to the campfire.");
            WilderlightHUD.Instance?.RefreshInventory(this);
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    Wood += amount;
                    break;
                case ResourceType.Food:
                    Food += amount;
                    break;
                case ResourceType.Crystal:
                    Crystal += amount;
                    break;
            }

            WilderlightHUD.Instance?.RefreshInventory(this);

            if (!campfireReady && HasEnoughSuppliesForCampfire())
            {
                campfireReady = true;
                WilderlightHUD.Instance?.SetObjective("Enough supplies collected. Follow the blue beacon back to the campfire.");

                CampfireInteraction campfire = UnityEngine.Object.FindAnyObjectByType<CampfireInteraction>();

                if (campfire != null)
                {
                    campfire.ShowReturnBeacon();
                }
            }
        }

        public bool HasEnoughSuppliesForCampfire()
        {
            return TotalSupplies >= suppliesNeededForCampfire;
        }

        public int SuppliesStillNeeded()
        {
            return Mathf.Max(0, suppliesNeededForCampfire - TotalSupplies);
        }
    }
}
