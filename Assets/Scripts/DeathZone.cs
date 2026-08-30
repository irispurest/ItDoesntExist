using Unity.VectorGraphics;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    private bool isDying;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying) return;
        
        Debug.Log(collision.gameObject.name);
        
        if (collision.TryGetComponent(out ManifestedObject manifestedObject))
            manifestedObject.Despawn();
        else if (collision.CompareTag("Player"))
        {
            sceneLoader.ReloadScene();
            isDying = true;
        }
    }
}
