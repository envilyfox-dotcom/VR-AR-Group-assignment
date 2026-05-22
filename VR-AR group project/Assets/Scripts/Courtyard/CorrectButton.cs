using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CorrectButton : MonoBehaviour
{
    [Header("References")]
    public LightToggleController lightController;
    public PuzzleMaster puzzleMaster;

    private XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (lightController == null || !lightController.IsOn()) return;

        puzzleMaster.CorrectButtonPressed();
        lightController.TurnOff();
    }
}