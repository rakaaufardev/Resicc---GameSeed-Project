using TMPro;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject uiItemDescPanel;
    [SerializeField] private GameObject uiButtonInteractPanel;
    [SerializeField] private GameObject uiButtonKeepThrowPanel;
    [SerializeField] private TextMeshProUGUI infoTextUI;

    [SerializeField] private float keepMoodValue = 10f;
    [SerializeField] private float keepEnergyValue = 10f;
    [SerializeField] private float throwMoodValue = 5f;
    [SerializeField] private float throwEnergyValue = 5f;

    [SerializeField] private bool isClueItem;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent; // Store original parent
    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.LogWarning("GameController not found in the scene.");
        }

        // Store the item's initial position, rotation, and parent
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent; // Store the original parent
    }

    public override void OnFocus()
    {
        if (!IsInteracting && infoTextUI != null)
        {
            infoTextUI.text = itemName;
            uiItemDescPanel.SetActive(true);
            uiButtonInteractPanel.SetActive(true);
        }
    }

    public override void OnInteract(Camera playerCamera)
    {
        if (IsInteracting) return; // Prevent interaction if already interacting

        if (playerCamera != null)
        {
            // Make item a child of player camera to follow camera movement
            transform.SetParent(playerCamera.transform);
            uiButtonKeepThrowPanel.SetActive(true);
            uiButtonInteractPanel.SetActive(false);

            // Reset item position and rotation to be in front of the camera
            transform.localPosition = new Vector3(0, 0, 1f); // Position units in front of the camera
            transform.localRotation = Quaternion.identity;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics
            }

            IsInteracting = true; // Mark as interacting
        }
    }

    public override void OnLoseFocus()
    {
        if (!IsInteracting && infoTextUI != null)
        {
            uiItemDescPanel.SetActive(false);
            uiButtonInteractPanel.SetActive(false);
        }
    }

    public override void OnKeep()
    {
        if (!IsInteracting) return; // Prevent keeping if not interacting

        gameController.KeepItem(keepMoodValue, keepEnergyValue, isClueItem);
        Destroy(gameObject);
        uiItemDescPanel.SetActive(false);
        uiButtonInteractPanel.SetActive(false);
        uiButtonKeepThrowPanel.SetActive(false);

        IsInteracting = false; // Reset interaction state
    }

    public override void OnThrow()
    {
        if (!IsInteracting) return; // Prevent throwing if not interacting

        gameController.ThrowItem(throwMoodValue, throwEnergyValue);
        Destroy(gameObject);
        uiItemDescPanel.SetActive(false);
        uiButtonInteractPanel.SetActive(false);
        uiButtonKeepThrowPanel.SetActive(false);

        IsInteracting = false; // Reset interaction state
    }

    private void Update()
    {
        // If item is being held and right-click is pressed, return item to original position
        if (IsInteracting && Input.GetMouseButtonDown(1))
        {
            ReturnToOriginalPosition();
            uiButtonKeepThrowPanel.SetActive(false);
        }
    }

    // Return the item to its original position, rotation, and parent
    private void ReturnToOriginalPosition()
    {
        // Remove item from the player (remove parent)
        transform.SetParent(originalParent);

        // Restore original position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Reactivate physics if applicable
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivate physics
        }

        IsInteracting = false; // Mark interaction as finished
    }
}