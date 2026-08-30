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
        inputSystem.Player.TurnTorch.performed += interactionController.TurnTorch;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Interact.performed -= interactionController.Interact;
        inputSystem.Player.TurnTorch.performed -= interactionController.TurnTorch;
    }

    private void FixedUpdate()
    {
        Vector2 input = inputSystem.Player.Move.ReadValue<Vector2>();
        movementController.ProcessInput(input);
    }
}
