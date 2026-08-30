using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class ExtinguishingFog : MonoBehaviour
{
    private BoxCollider2D collider;
    private SpriteRenderer renderer;
    
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        renderer =  GetComponent<SpriteRenderer>();
        
        collider.size = (Vector2)renderer.bounds.size - new Vector2(0.25f, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Fire fire) && fire.IsEnabled)
        {
            fire.Interact();
        }
    }
}
