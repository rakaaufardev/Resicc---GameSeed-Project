using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastChap : MonoBehaviour
{
    public AudioSource bgmAudioSource; // Drag the AudioSource into this field
    public AudioClip bgmClip; // Drag your BGM audio file here

    void Start()
    {
        // Play background music
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.loop = true; 
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM AudioSource or AudioClip is missing!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
