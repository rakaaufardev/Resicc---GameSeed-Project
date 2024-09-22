using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Button nextButton;
    public AudioSource bgmAudioSource; // Drag the AudioSource into this field
    public AudioClip bgmClip; // Drag your BGM audio file here

    void Start()
    {
        // Add listeners to each button
        nextButton.onClick.AddListener(OnNextButton);

        // Play background music
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.loop = true; // Makes the music loop
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM AudioSource or AudioClip is missing!");
        }
    }

    void OnNextButton()
    {
        SceneManager.LoadScene("Intro1");
    }
}