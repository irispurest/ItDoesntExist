using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HolyLight : MonoBehaviour
{
    [SerializeField] private Material fireMaterial;
    [SerializeField] private bool isEnabled;
    private List<ManifestedObject> affectedObjects;
    private int intensityPropertyID;

    private void Awake()
    {
        affectedObjects = new List<ManifestedObject>();
        intensityPropertyID = Shader.PropertyToID("_Intensity");
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void Extinguish()
    {
        isEnabled = false;
    }
}
