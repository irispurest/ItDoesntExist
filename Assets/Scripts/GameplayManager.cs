using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    [SerializeField] private MovementController movementController;
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Time.timeScale = 1;
        inputSystem = new InputSystem_Actions();
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Enable();
        inputSystem.UI.Disable();
        inputSystem.Player.Interact.performed += interactionController.Interact;
        inputSystem.Player.TurnTorch.performed += interactionController.TurnTorch;
        inputSystem.Player.Pause.performed += Pause;
        inputSystem.UI.Unpause.performed += Resume;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Interact.performed -= interactionController.Interact;
        inputSystem.Player.TurnTorch.performed -= interactionController.TurnTorch;
        inputSystem.Player.Pause.performed -= Pause;
        inputSystem.UI.Unpause.performed -= Resume;
    }

    private void FixedUpdate()
    {
        Vector2 input = inputSystem.Player.Move.ReadValue<Vector2>();
        movementController.ProcessInput(input);
    }

    private void Pause(InputAction.CallbackContext context)
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        inputSystem.UI.Enable();
        inputSystem.Player.Disable();
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void Resume(InputAction.CallbackContext context)
    {
        Resume();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        inputSystem.UI.Disable();
        inputSystem.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
