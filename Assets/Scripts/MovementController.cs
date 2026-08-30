using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float pushSpeed;
    [SerializeField] private float pushThreshold;
    [SerializeField] private string pushableTag;
    [SerializeField] private Transform character;
    private Animator animator; 
    private List<Rigidbody2D> pushables;
    private Rigidbody2D rb;
    private Rigidbody2D.SlideMovement slideMovement;
    private bool isPushing;

    private void Awake()
    {
        pushables = new List<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slideMovement = new Rigidbody2D.SlideMovement
        {
            surfaceUp = Vector2.zero,
            gravity = Vector2.zero,
            surfaceAnchor = Vector2.zero,
        };
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(pushableTag) && other.gameObject.TryGetComponent(out Rigidbody2D otherRb))
            pushables.Add(other.rigidbody);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody2D otherRb) && pushables.Contains(otherRb))
            pushables.Remove(otherRb);
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
            Rigidbody2D bestRb = null;
            Vector2 bestDir = Vector2.zero;
            
            foreach (var pushable in pushables)
            {
                Vector2 pushDirection = (pushable.position - rb.position).normalized;
                float dot = Vector3.Dot(input, pushDirection);
                
                if (dot < pushThreshold || dot < maxDot) continue;
                
                maxDot = dot;
                bestRb = pushable;
                bestDir = pushDirection;
            }

            if (maxDot == 0)
                isPushing = false;
            else
            {
                Vector2 direction = GetAlignedDirection(bestDir, input);
                bestRb.Slide(direction * pushSpeed, Time.fixedDeltaTime, slideMovement);
                rb.Slide(direction * pushSpeed, Time.fixedDeltaTime, slideMovement);
                isPushing = true;
            }
        }
        
        if (!isPushing)
            rb.Slide(input * speed, Time.fixedDeltaTime, slideMovement);
        
        animator.SetBool("isMoving", input.magnitude > 0);
        animator.SetBool("isPushing", isPushing);
    }

    private Vector2 GetAlignedDirection(Vector2 direction, Vector2 input)
    {
        return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector2(input.x, 0f) : new Vector2(0f, input.y);
    }
}
