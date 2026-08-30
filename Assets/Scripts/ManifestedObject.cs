using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ManifestedObject : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private string transparentTag;
    private SpriteRenderer[] childRenderers;
    private List<Collider2D> lightSources;
    private Collider2D collider;
    private ShadowCaster2D shadowCaster;
    private Vector3 originalPosition;
    private ParticleSystem particleSystem;
    private bool isAwake;
    public bool IsEnabled { private set; get; }
    public Action OnDespawn;
    
    private void Awake()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        lightSources = new List<Collider2D>();
        collider = GetComponent<Collider2D>();
        shadowCaster = GetComponent<ShadowCaster2D>();
        originalPosition = transform.position;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        
        IsEnabled = false;
        collider.enabled = false;
        shadowCaster.enabled = false;
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = false;
        isAwake = true;
    }

    private void FixedUpdate()
    {
        if (CheckObstacles())
            Despawn();
    }

    public void Despawn()
    {
        if (!isAwake) return;
        IsEnabled = false;
        collider.enabled = false;
        shadowCaster.enabled = false;
        particleSystem.Play();
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = false;
        OnDespawn?.Invoke();
    }

    public void Spawn()
    {
        if (!isAwake) return;
        transform.position = originalPosition;
        IsEnabled = true;
        collider.enabled = true;
        shadowCaster.enabled = true;
        particleSystem.Play();
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = true;
    }

    public bool CheckObstacles()
    {
        if (!IsEnabled || lightSources.Count == 0) return false;
        
        int lightSourceCount = lightSources.Count;
        
        foreach (Collider2D lightSource in lightSources)
        {
            Vector2 direction = (lightSource.transform.position - transform.position).normalized;
            float distance = (lightSource.transform.position - transform.position).magnitude;

            var hits = Physics2D.RaycastAll(transform.position, direction, distance, raycastLayerMask);

            Debug.Log("Check " + lightSource.name + ": hits " + hits.Length);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != collider && !hit.collider.CompareTag(transparentTag))
                {
                    Debug.Log("Obstacle found " + hit.collider.name);
                    lightSourceCount--;
                    break;
                }
            }
            
            Debug.Log("Available lights: " + lightSourceCount);
        }
        
        return lightSourceCount == 0;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Light")) return;
        
        lightSources.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Light") || !lightSources.Contains(other)) return;
        
        lightSources.Remove(other);
        if (lightSources.Count == 0 && IsEnabled) Despawn();
    }
}
