using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float mood = 100f;
    [SerializeField] private float energy = 100f;
    [SerializeField] private int clueItemsCollected = 0;
    [SerializeField] private int startingPoint = 20;

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
    private float minuteToRealTime = 0.5f;
    private float timer;
    private int maxGameDuration = 15; // 15 hours
    private int initialHour = 6; // Game starts at 6 AM
    [SerializeField] private bool isGameEnded = false; // Flag to check if game has ended

    void Start()
    {
        Minute = 0;
        Hour = initialHour;
        timer = minuteToRealTime;
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
                Debug.Log("Sekarang menit " + Minute);

                if (Minute >= 60)
                {
                    Hour++;
                    Minute = 0;
                    OnHourChanged?.Invoke();
                    Debug.Log("Sekarang jam " + Hour);

                    // Invoke time-specific events
                    if (Hour == 6 || Hour == 7)
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
    }

    public void KeepItem(float keepMoodValue, float keepEnergyValue, bool isClueItem)
    {
        mood += keepMoodValue;
        energy += keepEnergyValue;

        mood = Mathf.Clamp(mood, 0f, 100f);
        energy = Mathf.Clamp(energy, 0f, 100f);

        if (isClueItem)
        {
            clueItemsCollected++;
            if (clueItemsCollected > 10) clueItemsCollected = 10;
            Debug.Log("Clue item collected! Total clues: " + clueItemsCollected);
        }
    }

    public void EndGame()
    {
        isGameEnded = true; // Set the flag to stop the timer
        if (mood > 80f && clueItemsCollected >= 10)
        {
            // Good ending
            Debug.Log("Good Ending!");
        }
        else if (mood < 20f && clueItemsCollected <= 5)
        {
            // Bad ending
            Debug.Log("Bad Ending!");
        }
        else
        {
            // Default Ending
            Debug.Log("Neutral Ending!");
        }
    }
}
