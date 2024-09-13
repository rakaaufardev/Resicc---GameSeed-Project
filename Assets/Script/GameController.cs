using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private float mood = 100f;
    [SerializeField] private float energy = 100f;
    [SerializeField] private int clueItemsCollected = 0;
    [SerializeField] private int startingPoint = 20;

    //time
    //Action
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    //set time static 
    public static int Minute {get; private set;}
    public static int Hour {get; private set;}

    //0.5 second real time is 1 minute game time
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
    // Only update the timer if the game hasn't ended
    if (!isGameEnded)
    {
        // Update time
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
            }

            // Reset the timer for the next game minute
            timer = minuteToRealTime;
        }

        // Check if the game has reached 15 game hours or energy is depleted
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
