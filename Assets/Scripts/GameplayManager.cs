using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    [SerializeField] private MovementController movementController;
    [SerializeField] private InteractionController interactionController;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Interact.performed += interactionController.Interact;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Interact.performed -= interactionController.Interact;
    }

private void FixedUpdate()
    {
        movementController.ProcessInput(inputSystem.Player.Move.ReadValue<Vector2>());
    }
}
