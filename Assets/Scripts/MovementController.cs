using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform character;
    [SerializeField] private float pushSpeed;
    [SerializeField] private float pushThreshold;
    [SerializeField] private string pushableTag;
    private Animator animator; 
    private BoxCollider2D collider; 
    private List<Rigidbody2D> pushables;
    private Rigidbody2D rb;
    private Rigidbody2D.SlideMovement slideMovement;
    private bool isPushing;

    private Vector2 currentInput;
    private Vector2 pushDir;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        pushables = new List<Rigidbody2D>();
        slideMovement = new Rigidbody2D.SlideMovement
        {
            surfaceUp = Vector2.zero,
            gravity = Vector2.zero,
            surfaceAnchor = Vector2.zero,
        };
    }

    public void ProcessInput(Vector2 input)
    {
        if (input.x > 0)
            character.localScale = new Vector3(1, 1, 1);
        else if (input.x < 0)
            character.localScale = new Vector3(-1, 1, 1);

        if (pushables.Count == 0)
        {
            isPushing = false;
        }
        else
        {
            float maxDot = 0;
            Rigidbody2D bestRb = pushables[0];
            
            foreach (var pushable in pushables)
            {
                Collider2D[] colliders = new Collider2D[1];
                pushable.GetAttachedColliders(colliders);
                Vector2 pushDirection = (colliders[0].bounds.center - collider.bounds.center).normalized;
                float dot = Vector3.Dot(input, pushDirection);
                
                pushDir = pushDirection;
                currentInput = input;
                
                if (dot < pushThreshold || dot < maxDot) continue;
                
                maxDot = dot;
                bestRb = pushable;
            }

            if (maxDot == 0)
                isPushing = false;
            else
            {
                bestRb.Slide(input * pushSpeed, Time.fixedDeltaTime, slideMovement);
                rb.Slide(input * pushSpeed, Time.fixedDeltaTime, slideMovement);
                isPushing = true;
            }
        }
        
        if (!isPushing)
            rb.Slide(input * speed, Time.fixedDeltaTime, slideMovement);
        
        animator.SetBool("isMoving", input.magnitude > 0);
        animator.SetBool("isPushing", isPushing);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(pushableTag) && other.gameObject.TryGetComponent(out Rigidbody2D otherRb))
        {
            pushables.Add(otherRb);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody2D otherRb) && pushables.Contains(otherRb))
        {
            pushables.Remove(otherRb);
        }
    }
}
