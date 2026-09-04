using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Fire torchFire;
    private List<Interactable> interactables;

    private void Awake()
    {
        interactables = new List<Interactable>();
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (interactables.Count == 0) return;

        float minDistance = float.MaxValue;
        Interactable currentInteractable = null;
        
        foreach (var interactable in interactables)
        {
            float distance = (transform.position - interactable.transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                currentInteractable = interactable;
            }
        }
        
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
        else
        {
            foreach (Interactable interactable in interactables)
                if (interactable.TryGetComponent(out Fire fire) && fire.IsEnabled)
                    torchFire.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable))
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable>(out var interactable) && interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
        }
    }
}
