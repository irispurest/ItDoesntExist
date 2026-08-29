using System;
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
    [SerializeField] private bool isEnabled;
    [SerializeField] private float fadeDuration;
    
    private Material materialInstance;
    private float maxIntensity;
    private int intensityPropertyID;
    private bool isFading;

    private void Awake()
    {
        materialInstance = renderer.material;
        intensityPropertyID = Shader.PropertyToID("_Intensity");
        maxIntensity = light.intensity;

        if (!isEnabled)
        {
            lightTrigger.enabled = false;
            light.intensity = 0f;
            materialInstance.SetFloat(intensityPropertyID, 0f);
            foreach (var affectedObject in affectedObjects)
                affectedObject.Despawn();
        }
    }

    public override void Interact()
    {
        if (isFading) return;
        
        if (isEnabled)
            Extinguish();
        else
            LightFire();
    }

    public bool LightTorch()
    {
        return isEnabled;
    }

    private void Extinguish()
    {
        isEnabled = false;
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
        isEnabled = true;
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
