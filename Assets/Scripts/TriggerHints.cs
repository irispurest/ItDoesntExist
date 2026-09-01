using DG.Tweening;
using TMPro;
using UnityEngine;

public class TriggerHints : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float fadeDuration = 0.5f;
    private Tween tween;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        tween.Kill();
        tween = text.DOFade(1f, fadeDuration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        tween.Kill();
        tween = text.DOFade(0f, fadeDuration);
    }
}
