using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]  private float speed;
    private Rigidbody2D rb;
    private Rigidbody2D.SlideMovement slideMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        slideMovement = new Rigidbody2D.SlideMovement
        {
            surfaceUp = Vector2.zero,
            gravity = Vector2.zero,
            surfaceAnchor = Vector2.zero,
        };
    }
    
    public void ProcessInput(Vector2 input)
    {
        rb.Slide(input * speed, Time.fixedDeltaTime, slideMovement);
    }
}
