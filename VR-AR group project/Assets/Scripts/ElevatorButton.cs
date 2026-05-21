using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ElevatorButton : MonoBehaviour
{
    [Header("Settings")]
    public bool isRealButton = false;
    public int buttonIndex = 0;      // 0-3 for real buttons

    [Header("Visuals")]
    public Color normalColor = Color.white;
    public Color pressedColor = Color.green;
    public Color fakeColor = Color.red;

    private Renderer buttonRenderer;
    private bool hasBeenPressed = false;
    private XRBaseInteractable interactable;

    void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer != null)
            buttonRenderer.material.color = normalColor;

        interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnButtonPressed);
        else
            Debug.LogError("No XR Interactable on button: " + gameObject.name);
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnButtonPressed);
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (hasBeenPressed) return;
        hasBeenPressed = true;

        if (isRealButton)
        {
            Debug.Log(gameObject.name + " is a REAL button! Index: " + buttonIndex);

            if (buttonRenderer != null)
                buttonRenderer.material.color = pressedColor;

            ElevatorBuilder.Instance.ActivateButton(buttonIndex);
        }
        else
        {
            Debug.Log(gameObject.name + " is a FAKE button!");

            if (buttonRenderer != null)
                buttonRenderer.material.color = fakeColor;

            // optionally add fake feedback e.g. sound, shake
        }

        // prevent pressing again
        if (interactable != null)
            interactable.enabled = false;
    }
}