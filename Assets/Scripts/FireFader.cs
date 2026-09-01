using DG.Tweening;
using UnityEngine;

public class FireFader : MonoBehaviour
{
    [SerializeField] private float duration;
    private Renderer renderer;
    private Material materialInstance;
    private int intensityPropertyID;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        materialInstance = renderer.material;
        intensityPropertyID = Shader.PropertyToID("_Intensity");
    }

    public void Fade()
    {
        DOTween.To(
            () => materialInstance.GetFloat(intensityPropertyID),
            x => materialInstance.SetFloat(intensityPropertyID, x),
            0f,
            duration);
    }
}
