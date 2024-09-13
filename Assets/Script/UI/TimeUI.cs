using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{

    public TextMeshProUGUI timeText;
    
    private void OnEnable() 
    {
        GameController.OnMinuteChanged += UpdateTime;
        GameController.OnHourChanged += UpdateTime;
    }

    private void OnDisable() 
    {
        GameController.OnMinuteChanged -= UpdateTime;
        GameController.OnHourChanged -= UpdateTime;
    }

    private void UpdateTime()
    {
        timeText.text = $"{GameController.Hour:00}:{GameController.Minute:00}";
    }
}
