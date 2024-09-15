using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private float mood = 100f;
    [SerializeField] private float energy = 100f;
    [SerializeField] private int clueItemsCollected = 0;

    // UI Sliders for mood and energy
    [SerializeField] private Slider moodSlider;
    [SerializeField] private Slider energySlider;

    // Time
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnMorning;
    public static Action OnNoon;
    public static Action OnAfterNoon;
    public static Action OnEvening;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    // Time settings
    private float minuteToRealTime = 0.25f;
    private float timer;
    private int maxGameDuration = 15; // 15 hours
    private int initialHour = 9; // Game starts at 6 AM
    [SerializeField] private bool isGameEnded = false; // Flag to check if game has ended

    void Start()
    {
        Minute = 0;
        Hour = initialHour;
        timer = minuteToRealTime;

        // Initialize sliders
        moodSlider.minValue = 0; // Make sure the slider has a min value of 0
        moodSlider.maxValue = 100; // And max value of 100
        moodSlider.value = mood;   // Set its value initially

        energySlider.minValue = 0; // Same for the energy slider
        energySlider.maxValue = 100;
        energySlider.value = energy;
    }

    void Update()
    {
        if (!isGameEnded)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Minute++;
                OnMinuteChanged?.Invoke();

                if (Minute >= 60)
                {
                    Hour++;
                    Minute = 0;
                    OnHourChanged?.Invoke();

                    // Invoke time-specific events
                    if (Hour == 9 || Hour == 10)
                    {
                        OnMorning?.Invoke();
                    }
                    else if (Hour == 12)
                    {
                        OnNoon?.Invoke();
                    }
                    else if (Hour == 15)
                    {
                        OnAfterNoon?.Invoke();
                    }
                    else if (Hour == 18 || Hour == 19)
                    {
                        OnEvening?.Invoke();
                    }
                }

                timer = minuteToRealTime;
            }

            if (Hour >= initialHour + maxGameDuration || energy <= 0)
            {
                EndGame();
            }
        }
    }

    public void ThrowItem(float throwMoodValue, float throwEnergyValue)
    {
        mood += throwMoodValue;
        energy += throwEnergyValue;

        mood = Mathf.Clamp(mood, 0f, 100f);
        energy = Mathf.Clamp(energy, 0f, 100f);

        // Update UI sliders
        moodSlider.value = mood;
        energySlider.value = energy;
    }

    public void KeepItem(float keepMoodValue, float keepEnergyValue, bool isClueItem)
    {
        mood += keepMoodValue;
        energy += keepEnergyValue;

        mood = Mathf.Clamp(mood, 0f, 100f);
        energy = Mathf.Clamp(energy, 0f, 100f);

        // Update UI sliders
        moodSlider.value = mood;
        energySlider.value = energy;

        if (isClueItem)
        {
            clueItemsCollected++;
            if (clueItemsCollected > 10) clueItemsCollected = 10;
        }
    }

    public void EndGame()
    {
        isGameEnded = true; // Set the flag to stop the timer

        if (mood > 25f && clueItemsCollected >= 6)
        {
            // Good ending
            SceneManager.LoadScene("GoodEnding");
            Cursor.lockState = CursorLockMode.None;
        }
        else if (mood < 15f && clueItemsCollected <= 5)
        {
            // Bad ending
            SceneManager.LoadScene("BadEnding");
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            SceneManager.LoadScene("BadEnding");
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
