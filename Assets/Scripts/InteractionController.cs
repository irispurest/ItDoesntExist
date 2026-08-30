using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Fire torchFire;
    private Interactable currentInteractable;

    public void Interact(InputAction.CallbackContext context)
    {
        if (currentInteractable == null) return;

        if (currentInteractable.TryGetComponent(out Fire fire))
        {
            if (fire.IsEnabled || torchFire.IsEnabled)
                currentInteractable.Interact();
        }
        else 
            currentInteractable.Interact();
    }

    public void TurnTorch(InputAction.CallbackContext context)
    {
        if (torchFire.IsEnabled)
            torchFire.Interact();
        else if (currentInteractable != null && currentInteractable.TryGetComponent(out Fire fire) && fire.IsEnabled)
            torchFire.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}
