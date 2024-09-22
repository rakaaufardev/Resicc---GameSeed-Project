using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization; // Required to load scenes
using UnityEngine.UI; // Required for Button components

public class MainMenu : MonoBehaviour
{
    [FormerlySerializedAs("ButtonPanel")] [SerializeField] private GameObject buttonPanel;
    [FormerlySerializedAs("CreditsPanel")] [SerializeField] private GameObject creditsPanel;
    // Assign the buttons in the Inspector
    public Button playButton;
    public Button creditsButton;
    public Button exitButton;

    // Audio source for background music
    public AudioSource bgmAudioSource; // Drag the AudioSource into this field
    public AudioClip bgmClip; // Drag your BGM audio file here

    void Start()
    {
        // Add listeners to each button
        playButton.onClick.AddListener(OnPlayButton);
        creditsButton.onClick.AddListener(OnCreditsButton);
        exitButton.onClick.AddListener(OnExitButton);

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
    void OnPlayButton()
    {
        SceneManager.LoadScene("Tutorial"); 
    }


    void OnCreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    // Function when "Exit" button is clicked
    void OnExitButton()
    {
        Application.Quit(); // Quits the game
    }
}
