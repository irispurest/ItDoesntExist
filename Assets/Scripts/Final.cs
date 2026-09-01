using System.Collections;
using UnityEngine;

public class Final : MonoBehaviour
{
    [SerializeField] float screenDuration;
    [SerializeField] SceneLoader sceneLoader;
    
    private void Awake()
    {
        StartCoroutine(ToMainMenu());
    }

    IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(screenDuration);
        sceneLoader.ExitScene(0);
    }
}
