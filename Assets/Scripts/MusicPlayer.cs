using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MusicPlayer[] players = FindObjectsByType<MusicPlayer>();

        if (players.Length == 1)
        {
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
