using UnityEngine;

public class LightToggleController : MonoBehaviour
{
    [Header("References")]
    public GameObject[] areaLights;
    public GameObject[] quads;

    [Header("Buttons")]
    public GameObject turnOnButton;
    public GameObject turnOffButton;

    [Header("Warning UI")]
    public GameObject warningTextObject;

    private bool isOn = true;

    public void TurnOff() => SetState(false);
    public void TurnOn() => SetState(true);

    void Start()
    {
        // Sync isOn with actual state of quads in scene
        if (quads.Length > 0 && quads[0] != null)
            isOn = quads[0].activeSelf;

        Debug.Log($"{gameObject.name} starting isOn={isOn}");
    }

    public void Toggle()
    {
        isOn = !isOn;
        SetState(isOn);
    }

    private void SetState(bool state)
    {
        isOn = state;

        foreach (GameObject light in areaLights)
            if (light != null) light.SetActive(state);

        foreach (GameObject quad in quads)
            if (quad != null) quad.SetActive(state);

        if (turnOnButton != null) turnOnButton.SetActive(!state);
        if (turnOffButton != null) turnOffButton.SetActive(state);

        if (warningTextObject != null && state)
            warningTextObject.SetActive(false);
    }

    public bool IsOn() => isOn;
}