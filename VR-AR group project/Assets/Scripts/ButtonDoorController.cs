using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private HashSet<string> pressedButtons = new HashSet<string>();
    private Vector3 closedPosition;
    private bool isDoorOpen = false;
    private bool isMoving = false;

    void Start()
    {
        if (door) closedPosition = door.localPosition;
    }

    public void RegisterButtonPress(string buttonID)
    {
        if (!requiredButtonIDs.Contains(buttonID)) return;
        if (pressedButtons.Contains(buttonID)) return;

        pressedButtons.Add(buttonID);
        Debug.Log($"[Door] Button pressed: {buttonID} ({pressedButtons.Count}/{requiredButtonIDs.Count})");

        if (AllButtonsPressed() && !isDoorOpen && !isMoving)
            StartCoroutine(OpenDoor());
    }

    private bool AllButtonsPressed()
    {
        foreach (var id in requiredButtonIDs)
            if (!pressedButtons.Contains(id)) return false;
        return true;
    }

    private IEnumerator OpenDoor()
    {
        isMoving = true;
        isDoorOpen = true;

        if (audioSource && doorOpenSound)
        {
            audioSource.PlayOneShot(doorOpenSound);
            StartCoroutine(StopSoundAfter(soundDuration));
        }

        Vector3 startPos = door.localPosition;
        Vector3 targetPos = startPos + new Vector3(0, openYOffset, 0);
        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / openDuration;
            door.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        door.localPosition = targetPos;
        isMoving = false;
        Debug.Log("[Door] Door is now open!");
    }

    private IEnumerator StopSoundAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }
}