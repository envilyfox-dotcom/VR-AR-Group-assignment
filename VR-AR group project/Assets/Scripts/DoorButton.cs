using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [Header("Setup")]
    public string buttonID;
    public ButtonDoorController doorController;

    [Header("Button Visual")]
    public Renderer buttonRenderer;
    public Color pressedColor = Color.white;

    [Header("Light Bulb Indicator")]
    public Light indicatorLight;
    public Renderer bulbRenderer;
    public Material bulbOnMaterial;
    public Material bulbOffMaterial;

    private bool isPressed = false;
    private bool canBePressed = false;

    void Start()
    {
        if (buttonRenderer)
            _ = buttonRenderer.material.color;

        if (indicatorLight) indicatorLight.enabled = false;
        if (bulbRenderer && bulbOffMaterial)
            bulbRenderer.material = bulbOffMaterial;

        Invoke(nameof(EnableButton), 1f);
    }

    private void EnableButton() => canBePressed = true;

    public void OnButtonPressed()
    {
        if (!canBePressed || isPressed) return;
        isPressed = true;

        if (buttonRenderer)
            buttonRenderer.material.color = pressedColor;

        if (indicatorLight) indicatorLight.enabled = true;
        if (bulbRenderer && bulbOnMaterial)
            bulbRenderer.material = bulbOnMaterial;

        doorController?.RegisterButtonPress(buttonID);
    }
}