using UnityEngine;

namespace WilderlightForager
{
    public class ExitGateInteraction : InteractableObject
    {
        [SerializeField] private GameObject lockedVisual;
        [SerializeField] private GameObject openVisual;
        [SerializeField] private GameObject exitMarker;

        private bool unlocked;

        public void Configure(GameObject lockedGate, GameObject openGate, GameObject marker)
        {
            lockedVisual = lockedGate;
            openVisual = openGate;
            exitMarker = marker;
        }

        private void Start()
        {
            GameManager.Instance?.RegisterExitGate(this);
            SetVisualState(false);
        }

        public void Unlock()
        {
            unlocked = true;
            SetVisualState(true);
        }

        public override void Interact(GameObject player)
        {
            if (!unlocked)
            {
                WilderlightHUD.Instance?.SetObjective("The exit is locked. Light the campfire first.");
                WilderlightAudio.Instance?.PlayDenied(transform.position);
                return;
            }

            GameManager.Instance?.Win();
        }

        private void SetVisualState(bool isOpen)
        {
            if (lockedVisual != null)
            {
                lockedVisual.SetActive(!isOpen);
            }

            if (openVisual != null)
            {
                openVisual.SetActive(isOpen);
            }

            if (exitMarker != null)
            {
                exitMarker.SetActive(isOpen);
            }
        }
    }
}

