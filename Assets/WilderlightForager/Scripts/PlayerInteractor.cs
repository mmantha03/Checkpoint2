using UnityEngine;

namespace WilderlightForager
{
    public class PlayerInteractor : MonoBehaviour
    {
        private InteractableObject currentInteractable;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
            {
                return;
            }

            if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
            {
                currentInteractable.Interact(gameObject);
            }
        }

        public void SetCurrentInteractable(InteractableObject interactable)
        {
            currentInteractable = interactable;
            WilderlightHUD.Instance?.ShowPrompt(interactable.PromptMessage);
            WilderlightAudio.Instance?.PlayPrompt(transform.position);
        }

        public void ClearCurrentInteractable(InteractableObject interactable)
        {
            if (currentInteractable != interactable)
            {
                return;
            }

            currentInteractable = null;
            WilderlightHUD.Instance?.HidePrompt();
        }
    }
}
