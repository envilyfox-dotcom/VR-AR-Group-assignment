using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StayOpenDoor : MonoBehaviour
{
    [Header("References")]
    public GameObject handle;
    public Transform doorTransform;  // drag the door mesh directly here

    [Header("Settings")]
    public float openAngle = 90f;
    public float animationSpeed = 2f;

    public enum SwingAxis { X, Y, Z }
    public SwingAxis axis = SwingAxis.Y;  // change in inspector

    private bool hasOpened = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Start()
    {
        interactable = handle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnHandleGrabbed);
        else
            Debug.LogError("No XR Interactable found on: " + handle.name);
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnHandleGrabbed);
    }

    void OnHandleGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("Handle grabbed, opening door!");
        if (hasOpened) return;
        hasOpened = true;
        StartCoroutine(AnimateOpen());
    }

    IEnumerator AnimateOpen()
    {
        Quaternion startRot = doorTransform.localRotation;
        Quaternion endRot;

        if (axis == SwingAxis.X)
            endRot = Quaternion.Euler(openAngle, doorTransform.localEulerAngles.y, doorTransform.localEulerAngles.z);
        else if (axis == SwingAxis.Y)
            endRot = Quaternion.Euler(doorTransform.localEulerAngles.x, openAngle, doorTransform.localEulerAngles.z);
        else
            endRot = Quaternion.Euler(doorTransform.localEulerAngles.x, doorTransform.localEulerAngles.y, openAngle);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            doorTransform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        doorTransform.localRotation = endRot;
        interactable.enabled = false;
    }
}