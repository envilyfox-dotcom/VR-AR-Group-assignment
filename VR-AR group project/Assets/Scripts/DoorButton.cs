using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [Header("Setup")]
    public string buttonID;
    public ButtonDoorController doorController;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;
    public Color pressedColor = Color.white;

    private bool isPressed = false;
    private bool canBePressed = false;
    private Color originalColor;

    void Start()
    {
        if (buttonRenderer)
            originalColor = buttonRenderer.material.color;

        Invoke(nameof(EnableButton), 1f);
    }

    private void EnableButton()
    {
        canBePressed = true;
        Debug.Log($"[DoorButton] {buttonID} is ready!");
    }

    public void OnButtonPressed()
    {
        if (!canBePressed) return;
        if (isPressed) return;

        isPressed = true;

        if (buttonRenderer)
            buttonRenderer.material.color = pressedColor;

        doorController?.RegisterButtonPress(buttonID);
    }
}