using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const float MaxValue = 100f;
    private const int MaxClueItems = 10;
    private const float MoodThresholdForGoodEnding = 25f;
    private const float MoodThresholdForBadEnding = 15f;
    private const int MinClueItemsForGoodEnding = 6;
    private const int MaxClueItemsForBadEnding = 5;

    [SerializeField] private float mood = MaxValue;
    [SerializeField] private float energy = MaxValue;
    [SerializeField] private int clueItemsCollected;
    [SerializeField] private Slider moodSlider;
    [SerializeField] private Slider energySlider;

    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    private static Action _onMorning;
    private static Action _onNoon;
    private static Action _onAfterNoon;
    private static Action _onEvening;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private const float MinuteToRealTime = 0.25f;
    private const int MaxGameDuration = 15;
    private const int InitialHour = 9;
    private float timer;
    private bool isGameEnded;

    void Start()
    {
        InitializeTime();
        InitializeSliders();
    }

    void Update()
    {
        if (isGameEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            UpdateTime();
        }
        CheckGameEndConditions();
    }

    public void ThrowItem(float moodImpact, float energyImpact)
    {
        AdjustStats(moodImpact, energyImpact);
    }

    public void KeepItem(float moodImpact, float energyImpact, bool isClueItem)
    {
        AdjustStats(moodImpact, energyImpact);
        if (isClueItem)
        {
            clueItemsCollected = Mathf.Min(clueItemsCollected + 1, MaxClueItems);
        }
    }

    private void EndGame()
    {
        isGameEnded = true;
        if (mood > MoodThresholdForGoodEnding && clueItemsCollected >= MinClueItemsForGoodEnding)
        {
            LoadScene("GoodEnding");
        }
        else if (mood < MoodThresholdForBadEnding && clueItemsCollected <= MaxClueItemsForBadEnding)
        {
            LoadScene("BadEnding");
        }
        else
        {
            LoadScene("BadEnding");
        }
    }

    private void InitializeTime()
    {
        Minute = 0;
        Hour = InitialHour;
        timer = MinuteToRealTime;
    }

    private void InitializeSliders()
    {
        moodSlider.minValue = 0;
        moodSlider.maxValue = MaxValue;
        moodSlider.value = mood;

        energySlider.minValue = 0;
        energySlider.maxValue = MaxValue;
        energySlider.value = energy;
    }

    private void UpdateTime()
    {
        Minute++;
        OnMinuteChanged?.Invoke();
        if (Minute >= 60)
        {
            Hour++;
            Minute = 0;
            OnHourChanged?.Invoke();
            TriggerTimeEvents();
        }
        timer = MinuteToRealTime;
    }

    private void TriggerTimeEvents()
    {
        if (Hour == 9 || Hour == 10) { _onMorning?.Invoke(); }
        else if (Hour == 12) { _onNoon?.Invoke(); }
        else if (Hour == 15) { _onAfterNoon?.Invoke(); }
        else if (Hour == 18 || Hour == 19) { _onEvening?.Invoke(); }
    }

    private void AdjustStats(float moodImpact, float energyImpact)
    {
        mood = Mathf.Clamp(mood + moodImpact, 0f, MaxValue);
        energy = Mathf.Clamp(energy + energyImpact, 0f, MaxValue);
        moodSlider.value = mood;
        energySlider.value = energy;
    }

    private void CheckGameEndConditions()
    {
        if (Hour >= InitialHour + MaxGameDuration || energy <= 0)
        {
            EndGame();
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Cursor.lockState = CursorLockMode.None;
    }
}