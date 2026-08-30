using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushController : MonoBehaviour
{
    [SerializeField] private float pushSpeed;
    [SerializeField] private string pushableTag;
    
    private Animator animator; 
    private Rigidbody2D rb;
    private Rigidbody2D.SlideMovement slideMovement;
    private Rigidbody2D pushable;
    private Rigidbody2D currentPushable;
    public Action OnStartPushing;
    public Action OnStopPushing;
    private bool buttonPressed = false;
    private bool isPushing = false;

    private Vector2 currentInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slideMovement = new Rigidbody2D.SlideMovement
        {
            surfaceUp = Vector2.zero,
            gravity = Vector2.zero,
            surfaceAnchor = Vector2.zero,
        };
    }

    private void StartPushing()
    {
        Debug.Log("StartPushing");
        animator.SetBool("isPushing", true);
        isPushing = true;
        OnStartPushing?.Invoke();
    }

    private void StopPushing()
    {
        Debug.Log("StopPushing");
        animator.SetBool("isPushing", false);
        isPushing = false;
        if (pushableTag == null)
            currentPushable = null;
        OnStopPushing?.Invoke();
    }

    public void ButtonStart(InputAction.CallbackContext context)
    {
        buttonPressed = true;
        if (pushable != null)
        {
            StartPushing();
        }
    }

    public void ButtonCancel(InputAction.CallbackContext context)
    {
        buttonPressed = false;
        if (isPushing)
        {
            StopPushing();
        }
    }

    public void ProcessInput(Vector2 input)
    {
        currentInput = input;

        if (isPushing && input.magnitude == 0)
        {
            StopPushing();
            return;
        }
        if (!isPushing && input.magnitude > 0 && buttonPressed && pushable != null)
            StartPushing();
        
        currentPushable.Slide(input  * pushSpeed, Time.fixedDeltaTime, slideMovement);
        rb.Slide(input * pushSpeed, Time.fixedDeltaTime, slideMovement);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(pushableTag) && other.gameObject.TryGetComponent(out Rigidbody2D otherRb))
        {
            pushable = otherRb;
            currentPushable = otherRb;
            Debug.Log("Pushable: " + pushable.name);
            if (buttonPressed && currentInput.magnitude != 0)
            {
                StartPushing();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody2D otherRb) && pushable == otherRb && !isPushing)
        {
            pushable = null;
        }
    }
}
