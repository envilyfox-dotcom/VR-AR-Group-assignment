using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [Header("Correct Buttons (in order)")]
    public GameObject[] correctButtons; // all 4 in order

    private int currentStage = 0;

    void Start()
    {
        // Disable all except button 1
        for (int i = 1; i < correctButtons.Length; i++)
            correctButtons[i].SetActive(false);
    }

    public void CorrectButtonPressed()
    {
        currentStage++;
        Debug.Log("Stage advanced to: " + currentStage);
    }

    // Called by whichever LightToggleController turns lights back on
    public void OnLightsRestored()
    {
        Debug.Log("Lights restored, enabling button at stage: " + currentStage);
        if (currentStage < correctButtons.Length)
            correctButtons[currentStage].SetActive(true);
    }

    public int GetStage() => currentStage;
}