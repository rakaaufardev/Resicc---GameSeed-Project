using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] private bool isClueItem = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent; // Menyimpan parent original
    private bool isInteracting = false;
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
        originalParent = transform.parent; // Simpan parent awal item
    }

    public override void OnFocus()
    {
        if (!isInteracting && infoTextUI != null)
        {
            infoTextUI.text = itemName;
            uiItemDescPanel.SetActive(true);
            uiButtonInteractPanel.SetActive(true);
        }
    }

    public override void OnInteract(Camera playerCamera)
    {
        if (isInteracting) return; // Mencegah interaksi jika item sudah diinteraksi

        Debug.Log("Item interacted with!");

        if (playerCamera != null)
        {
            // Jadikan item child dari player camera agar ikut gerakan kamera
            transform.SetParent(playerCamera.transform);
            uiButtonKeepThrowPanel.SetActive(true);
            uiButtonInteractPanel.SetActive(false);

            // Reset posisi dan rotasi item agar tepat di depan kamera
            transform.localPosition = new Vector3(0, 0, 1.5f); // Posisi di depan kamera (1 unit)
            transform.localRotation = Quaternion.identity;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Nonaktifkan physics
            }

            isInteracting = true; // Tandai bahwa item sedang diinteraksi
        }
    }

    public override void OnLoseFocus()
    {
        if (!isInteracting && infoTextUI != null)
        {
            uiItemDescPanel.SetActive(false);
            uiButtonInteractPanel.SetActive(false);
        }
    }

    public override void OnKeep()
    {
        if (!isInteracting) return; // Mencegah keep jika tidak sedang interaksi

        Debug.Log("Item kept!");
        gameController.KeepItem(keepMoodValue, keepEnergyValue, isClueItem);
        Destroy(gameObject);
        uiItemDescPanel.SetActive(false);
        uiButtonInteractPanel.SetActive(true);
        uiButtonKeepThrowPanel.SetActive(false);
    }

    public override void OnThrow()
    {
        if (!isInteracting) return; // Mencegah throw jika tidak sedang interaksi

        Debug.Log("Item thrown!");
        gameController.ThrowItem(throwMoodValue, throwEnergyValue);
        Destroy(gameObject);
        uiItemDescPanel.SetActive(false);
        uiButtonInteractPanel.SetActive(true);
        uiButtonKeepThrowPanel.SetActive(false);
    }

    private void Update()
    {
        // Jika item sedang dipegang dan klik kanan ditekan, kembalikan item ke tempat asal
        if (isInteracting && Input.GetMouseButtonDown(1))
        {
            ReturnToOriginalPosition();
            uiButtonKeepThrowPanel.SetActive(false);
        }
    }

    // Mengembalikan item ke posisi, rotasi, dan parent awal
    private void ReturnToOriginalPosition()
    {
        // Lepaskan item dari player (hapus parent)
        transform.SetParent(originalParent);

        // Kembalikan posisi dan rotasi awal
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Aktifkan kembali physics jika ada
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Aktifkan physics
        }

        isInteracting = false; // Tandai bahwa interaksi telah selesai
    }
}
