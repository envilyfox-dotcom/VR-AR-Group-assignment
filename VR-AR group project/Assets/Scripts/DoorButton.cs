using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [Header("Setup")]
    public string buttonID;  // "btn_red", "btn_blue", or "btn_green"
    public ButtonDoorController doorController;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public Color pressedColor = Color.white;

    private bool isPressed = false;
    private Color originalColor;

    void Start()
    {
        if (buttonRenderer)
            originalColor = buttonRenderer.material.color;
    }

    // Called by XR Simple Interactable → Select Entered event
    public void OnButtonPressed()
    {
        if (isPressed) return;
        isPressed = true;

        // Flash white to show it's been activated
        if (buttonRenderer)
            buttonRenderer.material.color = pressedColor;

        doorController?.RegisterButtonPress(buttonID);
    }
}