using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    [SerializeField] private MovementController movementController;
    
    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void FixedUpdate()
    {
        movementController.ProcessInput(inputSystem.Player.Move.ReadValue<Vector2>());
    }
}
