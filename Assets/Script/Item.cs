using TMPro;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI infoTextUI;

    [SerializeField] private float keepMoodValue = 10f;
    [SerializeField] private float keepEnergyValue = 10f;
    [SerializeField] private float throwMoodValue = 5f;
    [SerializeField] private float throwEnergyValue = 5f;

    [SerializeField] private bool isClueItem = false;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController == null)
        {
            Debug.LogWarning("GameController not found in the scene.");
        }
    }

    public override void OnFocus()
    {
        if (infoTextUI != null)
        {
            infoTextUI.text = itemName;
            uiPanel.SetActive(true);
        }
    }

    public override void OnInteract()
    {
        Debug.Log("Item interacted with!");
    }

    public override void OnLoseFocus()
    {
        if (infoTextUI != null)
        {
            uiPanel.SetActive(false);
        }
    }

    public override void OnKeep()
    {
        // Logika untuk menyimpan item
        Debug.Log("Item kept!");

        // Panggil GameController untuk meng-update game state
        gameController.KeepItem(keepMoodValue, keepEnergyValue, isClueItem);
    }

    public override void OnThrow()
    {
        // Logika untuk membuang item
        Debug.Log("Item thrown!");

        // Panggil GameController untuk meng-update game state
        gameController.ThrowItem(throwMoodValue, throwEnergyValue);
    }

    
}
