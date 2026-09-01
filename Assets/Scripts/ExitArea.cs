using UnityEngine;

public class ExitArea : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] private int nextScene;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sceneLoader.ExitScene(nextScene);
    }
}
