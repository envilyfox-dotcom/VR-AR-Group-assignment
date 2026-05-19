using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // ADD THIS

public class ButtonDoorController : MonoBehaviour
{
    [Header("Required Button IDs")]
    public List<string> requiredButtonIDs = new List<string> { "btn_red", "btn_blue", "btn_green" };

    [Header("Door Settings")]
    public Transform door;
    public float openYOffset = 3f;
    public float openDuration = 6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public float soundDuration = 7f;

    [Header("Popup Text")]                          // ADD THIS BLOCK
    public TextMeshProUGUI popupText;
    public float popupDuration = 2f;
    private Coroutine hideCoroutine;

    private HashSet<string> pressedButtons = new HashSet<string>();
    private Vector3 closedPosition;
    private bool isDoorOpen = false;
    private bool isMoving = false;

    void Start()
    {
        if (door) closedPosition = door.localPosition;
        if (popupText) popupText.gameObject.SetActive(false);  // ADD THIS
    }

    public void RegisterButtonPress(string buttonID)
    {
        if (!requiredButtonIDs.Contains(buttonID)) return;
        if (pressedButtons.Contains(buttonID)) return;

        pressedButtons.Add(buttonID);
        Debug.Log($"[Door] Button pressed: {buttonID} ({pressedButtons.Count}/{requiredButtonIDs.Count})");

        ShowPopup();  // ADD THIS

        if (AllButtonsPressed() && !isDoorOpen && !isMoving)
            StartCoroutine(OpenDoor());
    }

    // ADD THIS ENTIRE METHOD
    private void ShowPopup()
    {
        if (!popupText) return;

        int total = requiredButtonIDs.Count;
        int found = pressedButtons.Count;

        popupText.text = found >= total
            ? "All buttons found!"
            : $"{found}/{total} buttons found";

        popupText.gameObject.SetActive(true);

        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HidePopupAfterDelay());
    }

    // ADD THIS ENTIRE METHOD
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        if (popupText) popupText.gameObject.SetActive(false);
    }

    // ... rest of your existing code unchanged