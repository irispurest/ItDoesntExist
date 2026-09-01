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
    public bool extinguishable;
    [SerializeField] private bool isLightingFires;
    [SerializeField] private FireLighter fireLighter;
    [SerializeField] private bool isNonLightable;
    [SerializeField] private AudioSource blowSound;
    [SerializeField] private AudioSource lightUpSound;
    
    private List<Fire> surroundingFires;
    
    private Material materialInstance;
    private float maxFireIntensity;
    private float maxIntensity;
    private int intensityPropertyID;
    private bool isFading;
    
    [field: SerializeField] public bool IsEnabled { private set; get; }

    private void Awake()
    {
        materialInstance = renderer.material;
        intensityPropertyID = Shader.PropertyToID("_Intensity");
        maxFireIntensity = materialInstance.GetFloat(intensityPropertyID);
        maxIntensity = light.intensity;
        surroundingFires = new List<Fire>();

        if (isLightingFires)
        {
            fireLighter.OnEnterFire += EnterFire;
            fireLighter.OnExitFire += ExitFire;
        }
    }

    private void EnterFire(Fire fire)
    {
        surroundingFires.Add(fire);
        if (!fire.IsEnabled && IsEnabled)
            LightFire();
    }

    private void ExitFire(Fire fire)
    {
        surroundingFires.Remove(fire);
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
            affectedObject.OnDespawn += () =>
            {
                if (gameObject != null && gameObject.activeInHierarchy)
                    StartCoroutine(TrySpawn(affectedObject));
            };
        }
    }

    private void OnDisable()
    {
        foreach (var affectedObject in affectedObjects)
        {
            affectedObject.OnDespawn = null;
        }
    }

    IEnumerator TrySpawn(ManifestedObject obj)
    {
        yield return new WaitForEndOfFrame();
        if (obj != null && IsEnabled && !obj.IsEnabled && !obj.CheckObstacles())
            obj.Spawn();
    }

    public override void Interact()
    {
        if (!extinguishable) return;
        if (isFading) return;
        
        if (IsEnabled)
            Extinguish();
        else if (!isNonLightable)
            LightFire();
    }

    private void Extinguish()
    {
        IsEnabled = false;
        isFading = true;
        blowSound.Play();

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
        lightUpSound.Play();
        
        foreach (var affectedObject in affectedObjects)
        {
            if (!affectedObject.IsEnabled)
                affectedObject.Spawn();
        }

        foreach (var fire in surroundingFires)
        {
            if (!fire.IsEnabled)
                fire.LightFire();
        }
        
        DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            maxIntensity,
            fadeDuration);
        
        DOTween.To(
                () => materialInstance.GetFloat(intensityPropertyID),
                x => materialInstance.SetFloat(intensityPropertyID, x),
                maxFireIntensity,
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
