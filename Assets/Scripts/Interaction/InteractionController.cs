using UnityEngine;
using UnityEngine.EventSystems;
using VirtualEyeClinic.UI;

namespace VirtualEyeClinic.Interaction
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 4f;
        [SerializeField] private LayerMask interactableLayer = ~0;
        [SerializeField] private Camera playerCamera;

        private IInteractable currentInteractable;
        private InteractableHighlight currentHighlight;

        private void Update()
        {
            HandleRaycast();
            HandleInteractInput();
        }

        private void HandleRaycast()
        {
            if (playerCamera == null)
            {
                return;
            }

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                IInteractable interactable = FindInteractable(hit.collider);

                if (interactable != null)
                {
                    if (interactable != currentInteractable)
                    {
                        SetHighlight(null);
                        currentInteractable = interactable;
                        currentHighlight = hit.collider.GetComponentInParent<InteractableHighlight>();
                        SetHighlight(currentHighlight);
                        UIManager.Instance?.ShowCrosshairPrompt(currentInteractable.GetInteractionPrompt());
                    }

                    return;
                }
            }

            ClearInteractable();
        }

        private void HandleInteractInput()
        {
            if (currentInteractable == null)
            {
                return;
            }

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began && CanInteract(touch.fingerId, touch.position))
                    {
                        currentInteractable.Interact();
                        break;
                    }
                }
            }
#if UNITY_EDITOR
            else if (Input.GetMouseButtonDown(0) && CanInteract(-1, Input.mousePosition))
            {
                currentInteractable.Interact();
            }
#endif
        }

        private bool CanInteract(int pointerId, Vector2 screenPosition)
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(pointerId))
            {
                return false;
            }

            float normalizedX = screenPosition.x / Screen.width;
            if (normalizedX < 0.55f)
            {
                return false;
            }

            return true;
        }

        private void SetHighlight(InteractableHighlight highlight)
        {
            if (currentHighlight != null && currentHighlight != highlight)
            {
                currentHighlight.SetHighlighted(false);
            }

            currentHighlight = highlight;

            if (currentHighlight != null)
            {
                currentHighlight.SetHighlighted(true);
            }
        }

        private void ClearInteractable()
        {
            if (currentInteractable == null)
            {
                return;
            }

            currentInteractable = null;
            SetHighlight(null);
            UIManager.Instance?.HideCrosshairPrompt();
        }

        private static IInteractable FindInteractable(Collider collider)
        {
            MonoBehaviour[] behaviours = collider.GetComponentsInParent<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is IInteractable interactable)
                {
                    return interactable;
                }
            }

            return null;
        }
    }
}
