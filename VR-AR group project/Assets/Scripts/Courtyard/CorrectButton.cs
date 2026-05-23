// CorrectButton.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CorrectButton : MonoBehaviour
{
    [Header("References")]
    public LightToggleController lightController;
    public PuzzleMaster puzzleMaster;

    [Header("Tag of buttons to activate after this is pressed")]
    public string nextButtonsTag; // e.g. "BlueButton"

    private XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (lightController == null || puzzleMaster == null) return;

        // Removed isOn check — just always fire
        puzzleMaster.CorrectButtonPressed();
        lightController.TurnOff();

        if (!string.IsNullOrEmpty(nextButtonsTag))
        {
            GameObject[] nextButtons = GameObject.FindGameObjectsWithTag(nextButtonsTag);
            Debug.Log($"Found {nextButtons.Length} buttons with tag '{nextButtonsTag}'");
            foreach (GameObject btn in nextButtons)
                btn.SetActive(true);
        }
    }
}