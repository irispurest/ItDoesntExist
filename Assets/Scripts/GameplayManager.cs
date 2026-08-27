using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    [SerializeField] private MovementController movementController;
    
    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }
    
    private void Update()
}
