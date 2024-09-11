using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float mood = 100f;
    [SerializeField] private float energy = 100f;
    [SerializeField] private int clueItemsCollected = 0;
    [SerializeField] private float countdownTimer = 600f; // 10 minutes countdown
    
    [SerializeField] private int startingPoint = 20;

    void Update()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0 || energy <= 0)
        {
            EndGame();
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
        if (mood > 80f && clueItemsCollected > 10)
        {
            // Good ending
            Debug.Log("Good Ending!");
        }
        else if (mood < 20f && clueItemsCollected < 10)
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
