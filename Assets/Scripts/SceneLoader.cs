using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration;
    private Tween tween;

    private void Start()
    {
        fadeImage.color = Color.black;
        EnterScene();
    }
    
    private void SwitchScene(int scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    
    public void ExitScene(int scene)
    {
        tween.Kill();
        tween = fadeImage.DOFade(1f, fadeDuration)
            .SetUpdate(UpdateType.Normal, true)
            .OnComplete(() => SwitchScene(scene));
    }

    public void ReloadScene()
    {
        ExitScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EnterScene()
    {
        tween.Kill();
        tween = fadeImage.DOFade(0f, fadeDuration)
            .SetUpdate(UpdateType.Normal, true);
    }
}