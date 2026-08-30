using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform character;
    private Animator animator; 
    private List<Rigidbody2D> pushables;
    private Rigidbody2D rb;
    private Rigidbody2D.SlideMovement slideMovement;

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

    public void ProcessInput(Vector2 input)
    {
        if (input.x > 0)
            character.localScale = new Vector3(1, 1, 1);
        else if (input.x < 0)
            character.localScale = new Vector3(-1, 1, 1);

        rb.Slide(input * speed, Time.fixedDeltaTime, slideMovement);
        
        animator.SetBool("isMoving", input.magnitude > 0);
    }
}
