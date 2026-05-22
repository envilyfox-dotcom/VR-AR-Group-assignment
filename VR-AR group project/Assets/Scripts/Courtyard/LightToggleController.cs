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

    [Header("Master Controller")]
    public PuzzleMaster puzzleMaster;

    private bool isOn = true;

    public void TurnOff() => SetState(false);
    public void TurnOn() => SetState(true);

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

        // Tell the master to unlock the next correct button
        if (state && puzzleMaster != null)
            puzzleMaster.OnLightsRestored();
    }

    public bool IsOn() => isOn;
}
