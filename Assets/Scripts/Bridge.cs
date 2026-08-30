using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private Collider2D deathTrigger;
    private ManifestedObject manifestedObject;
    
    private void Awake()
    {
        manifestedObject = GetComponent<ManifestedObject>();
    }

    private void OnEnable()
    {
        manifestedObject.OnDespawn += OnDespawn;
        manifestedObject.OnSpawn += OnSpawn;
    }

    private void OnDisable()
    {
        manifestedObject.OnDespawn -= OnDespawn;
        manifestedObject.OnSpawn -= OnSpawn;
    }

    private void OnSpawn()
    {
        deathTrigger.enabled = false;
        collider.isTrigger = true;
        collider.enabled = true;
    }

    private void OnDespawn()
    {
        deathTrigger.enabled = true;
        collider.isTrigger = false;
        collider.enabled = true;
    }
}
