using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Remove if not using XR Toolkit

public class LightButton : MonoBehaviour
{
    public enum ButtonAction { TurnOn, TurnOff, Toggle }

    [Header("Settings")]
    public LightToggleController controller;
    public ButtonAction action = ButtonAction.Toggle;

    // --- Option A: XR Interaction Toolkit ---
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnButtonPressed);
    }

    // --- Option B: Physics-based (no XR Toolkit needed) ---
    void OnTriggerEnter(Collider other)
    {
        // Fires when the VR controller/hand enters the button collider
        if (other.CompareTag("PlayerHand") || other.CompareTag("GameController"))
            PerformAction();
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        PerformAction();
    }

    private void PerformAction()
    {
        if (controller == null) return;

        switch (action)
        {
            case ButtonAction.TurnOn: controller.TurnOn(); break;
            case ButtonAction.TurnOff: controller.TurnOff(); break;
            case ButtonAction.Toggle: controller.Toggle(); break;
        }
    }
}