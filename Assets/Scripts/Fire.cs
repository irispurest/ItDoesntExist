using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Fire : Interactable
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private CircleCollider2D lightTrigger;
    [SerializeField] private Light2D light;
    [SerializeField] private List<ManifestedObject> affectedObjects;
    [SerializeField] private float fadeDuration;
    [SerializeField] private bool extinguishable;
    
    private Material materialInstance;
    private float maxIntensity;
    private int intensityPropertyID;
    private bool isFading;
    
    [field: SerializeField] public bool IsEnabled { private set; get; }

    private void Awake()
    {
        materialInstance = renderer.material;
        intensityPropertyID = Shader.PropertyToID("_Intensity");
        maxIntensity = light.intensity;
    }

    private void Start()
    {
        if (!IsEnabled)
        {
            lightTrigger.enabled = false;
            light.intensity = 0f;
            materialInstance.SetFloat(intensityPropertyID, 0f);
            foreach (var affectedObject in affectedObjects)
                affectedObject.Despawn();
        }
        
        foreach (var affectedObject in affectedObjects)
        {
            affectedObject.OnDespawn += () => StartCoroutine(TrySpawn(affectedObject));
        }
    }

    IEnumerator TrySpawn(ManifestedObject obj)
    {
        yield return new WaitForEndOfFrame();
        if (IsEnabled && !obj.CheckObstacles())
            obj.Spawn();
    }

    public override void Interact()
    {
        if (!extinguishable) return;
        if (isFading) return;
        
        if (IsEnabled)
            Extinguish();
        else
            LightFire();
    }

    private void Extinguish()
    {
        IsEnabled = false;
        isFading = true;

        DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            0f,
            fadeDuration);
        
        DOTween.To(
                () => materialInstance.GetFloat(intensityPropertyID),
                x => materialInstance.SetFloat(intensityPropertyID, x),
                0f,
                fadeDuration)
            .OnComplete(FinishExtinguish);
    }

    private void LightFire()
    {
        IsEnabled = true;
        isFading = true;
        lightTrigger.enabled = true;
        
        foreach (var affectedObject in affectedObjects)
        {
            Debug.Log(affectedObject.IsEnabled);
            if (!affectedObject.IsEnabled)
                affectedObject.Spawn();
        }
        
        DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            maxIntensity,
            fadeDuration);
        
        DOTween.To(
                () => materialInstance.GetFloat(intensityPropertyID),
                x => materialInstance.SetFloat(intensityPropertyID, x),
                1f,
                fadeDuration)
            .OnComplete(FinishLighting);
    }

    private void FinishLighting()
    {
        isFading = false;
    }

    private void FinishExtinguish()
    {
        lightTrigger.enabled = false;
        isFading = false;
    }
}
