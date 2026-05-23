using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    public GameObject handle;
    public Transform doorTransform;

    [Header("Settings")]
    public float openAngle = 90f;
    public float animationSpeed = 2f;

    public enum SwingAxis { X, Y, Z }
    public SwingAxis axis = SwingAxis.Y;

    private bool isOpen = false;
    private bool isAnimating = false;
    private float closedAngle;
    private XRBaseInteractable interactable;

    public AudioClip openSound;
    public AudioClip closeSound;

    void Start()
    {
        // store the starting angle as the closed angle
        if (axis == SwingAxis.X)
            closedAngle = doorTransform.localEulerAngles.x;
        else if (axis == SwingAxis.Y)
            closedAngle = doorTransform.localEulerAngles.y;
        else
            closedAngle = doorTransform.localEulerAngles.z;

        interactable = handle.GetComponent<XRBaseInteractable>();
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
        if (isAnimating) return;
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Opening door..." : "Closing door...");
        StartCoroutine(AnimateDoor(isOpen ? openAngle : closedAngle));
    }

    IEnumerator AnimateDoor(float targetAngle)
    {
        isAnimating = true;
        Quaternion startRot = doorTransform.localRotation;
        Quaternion endRot;

        if (axis == SwingAxis.X)
            endRot = Quaternion.Euler(targetAngle, doorTransform.localEulerAngles.y, doorTransform.localEulerAngles.z);
        else if (axis == SwingAxis.Y)
            endRot = Quaternion.Euler(doorTransform.localEulerAngles.x, targetAngle, doorTransform.localEulerAngles.z);
        else
            endRot = Quaternion.Euler(doorTransform.localEulerAngles.x, doorTransform.localEulerAngles.y, targetAngle);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            doorTransform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        doorTransform.localRotation = endRot;
        isAnimating = false;
    }
}