// LightButton.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LightButton : MonoBehaviour
{
    public enum ButtonAction { TurnOn, TurnOff, Toggle }

    [Header("Settings")]
    public LightToggleController controller;
    public ButtonAction action = ButtonAction.TurnOn;

    [Header("Puzzle")]
    public PuzzleMaster puzzleMaster;
    public string colorTag; // "Blue", "Red", "Black"

    [Header("Disable self after pressed")]
    public bool disableAfterPress = true;

    private XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnButtonPressed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand") || other.CompareTag("GameController"))
            PerformAction();
    }

    private void OnButtonPressed(SelectEnterEventArgs args) => PerformAction();

    private void PerformAction()
    {
        if (controller == null) return;

        switch (action)
        {
            case ButtonAction.TurnOn: controller.TurnOn(); break;
            case ButtonAction.TurnOff: controller.TurnOff(); break;
            case ButtonAction.Toggle: controller.Toggle(); break;
        }

        // Removed controller.IsOn() check — TurnOn always makes it true
        if (puzzleMaster != null && !string.IsNullOrEmpty(colorTag) && action == ButtonAction.TurnOn)
            puzzleMaster.OnLightsRestored(colorTag);
        else
            Debug.Log($"OnLightsRestored NOT called — puzzleMaster={puzzleMaster}, action={action}, colorTag='{colorTag}'");
    }
}