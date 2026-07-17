using UnityEngine;

namespace WilderlightForager
{
    public abstract class InteractableObject : MonoBehaviour
    {
        [SerializeField] private string promptMessage = "Press E";

        public string PromptMessage => promptMessage;

        public abstract void Interact(GameObject player);

        public void SetPromptMessage(string message)
        {
            promptMessage = message;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerInteractor interactor = other.GetComponentInParent<PlayerInteractor>();

            if (interactor != null)
            {
                interactor.SetCurrentInteractable(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerInteractor interactor = other.GetComponentInParent<PlayerInteractor>();

            if (interactor != null)
            {
                interactor.ClearCurrentInteractable(this);
            }
        }
    }
}
