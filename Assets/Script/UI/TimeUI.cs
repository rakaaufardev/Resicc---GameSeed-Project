using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{

    public TextMeshProUGUI timeText;

    // UI settings
    [SerializeField] private Image timePanelImage; // Reference to the UI Image component
    [SerializeField] private Sprite morningSprite; // Sprite for 6 AM
    [SerializeField] private Sprite noonSprite; // Sprite for 12 PM
    [SerializeField] private Sprite afternoonSprite; // Sprite for 3 PM
    [SerializeField] private Sprite eveningSprite; // Sprite for 6 PM

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
        UpdateTimePanelImage();
    }

    private void UpdateTimePanelImage()
    {
        // Update the UI panel image based on the hour
        if (GameController.Hour == 6 || GameController.Hour == 7) // Adjust for morning time
        {
            timePanelImage.sprite = morningSprite;
        }
        else if (GameController.Hour == 12)
        {
            timePanelImage.sprite = noonSprite;
        }
        else if (GameController.Hour == 15)
        {
            timePanelImage.sprite = afternoonSprite;
        }
        else if (GameController.Hour == 18 || GameController.Hour == 19) // Adjust for evening time
        {
            timePanelImage.sprite = eveningSprite;
        }
    }
}
