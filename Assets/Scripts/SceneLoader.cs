using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration;
    private bool isFading;

    private void Start()
    {
        fadeImage.color = Color.black;
        EnterScene();
    }
    
    private void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    
    public void ExitScene(int scene)
    {
        if (isFading) return;
        isFading = true;
        Debug.Log("Exiting scene");
        fadeImage.DOFade(1.0f, fadeDuration).OnComplete(() => SwitchScene(scene));
    }

    public void ReloadScene()
    {
        ExitScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EnterScene()
    {
        if (isFading) return;
        isFading = true;
        fadeImage.DOFade(0.0f, fadeDuration).OnComplete(() => isFading = false);
    }
}