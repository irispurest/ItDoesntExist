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
    public bool IsEnabled { private set; get; }
    public Action OnDespawn;

    private Vector2 hitPoint;
    
    private void Awake()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        lightSources = new List<Collider2D>();
        shadowCaster = GetComponent<ShadowCaster2D>();
        originalPosition = transform.position;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        collider = GetComponent<Collider2D>();
        IsEnabled = true;
    }

    private void FixedUpdate()
    {
        if (CheckObstacles())
        {
            Despawn();
        }
    }

    public void Despawn()
    {
        particleSystem.Play();
        IsEnabled = false;
        collider.enabled = false;
        shadowCaster.enabled = false;
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = false;
        OnDespawn?.Invoke();
    }

    public void Spawn()
    {
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
            Vector2 direction = (lightSource.transform.position - collider.bounds.center).normalized;
            float distance = (lightSource.transform.position - collider.bounds.center).magnitude;

            var hits = Physics2D.RaycastAll(collider.bounds.center, direction, distance, raycastLayerMask);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != collider && hit.collider.TryGetComponent(out BlockLight _))
                {
                    lightSourceCount--;
                    hitPoint = hit.point;
                    break;
                }
            }
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
        if (lightSources.Count == 0 && IsEnabled)
        {
            Despawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.1f);
    }
}
