using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    [SerializeField] private MovementController movementController;
    [SerializeField] private PushController pushController;
    [SerializeField] private InteractionController interactionController;
    
    private bool isPushing;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Interact.performed += interactionController.Interact;
        inputSystem.Player.TurnTorch.performed += interactionController.TurnTorch;
        inputSystem.Player.Grab.started += pushController.ButtonStart;
        inputSystem.Player.Grab.canceled += pushController.ButtonCancel;
        pushController.OnStartPushing += () => { isPushing = true; };
        pushController.OnStopPushing += () => { isPushing = false; };
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Interact.performed -= interactionController.Interact;
        inputSystem.Player.TurnTorch.performed -= interactionController.TurnTorch;
        inputSystem.Player.Grab.started -= pushController.ButtonStart;
        inputSystem.Player.Grab.canceled -= pushController.ButtonCancel;
    }

    private void FixedUpdate()
    {
        Vector2 input = inputSystem.Player.Move.ReadValue<Vector2>();
        
        if (!isPushing)
            movementController.ProcessInput(input);
        
        Debug.Log(input.ToString());
        pushController.ProcessInput(input);
    }
}
