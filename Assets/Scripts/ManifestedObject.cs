using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ManifestedObject : MonoBehaviour
{
    private SpriteRenderer[] childRenderers;
    private List<Collider2D> lightSources;
    private Collider2D collider;
    private ShadowCaster2D shadowCaster;
    private Vector3 originalPosition;
    private ParticleSystem particleSystem;
    public bool IsEnabled { private set; get; }
    
    private void Awake()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        lightSources = new List<Collider2D>();
        collider = GetComponent<Collider2D>();
        shadowCaster = GetComponent<ShadowCaster2D>();
        originalPosition = transform.position;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        IsEnabled = true;
    }
    
    public void Despawn()
    {
        Debug.Log("Despawn");
        IsEnabled = false;
        collider.enabled = false;
        shadowCaster.enabled = false;
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = false;
    }

    public void Spawn()
    {
        Debug.Log("Spawn");
        transform.position = originalPosition;
        IsEnabled = true;
        collider.enabled = true;
        shadowCaster.enabled = true;
        particleSystem.Play();
        foreach (SpriteRenderer renderer in childRenderers)
            renderer.enabled = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Light")) return;
        
        lightSources.Add(other);
        Debug.Log("Lighted by " + other.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Light") || !lightSources.Contains(other)) return;
        
        Debug.Log("No more lighted by " + other.name);
        lightSources.Remove(other);
        if (lightSources.Count == 0) Despawn();
    }
}
