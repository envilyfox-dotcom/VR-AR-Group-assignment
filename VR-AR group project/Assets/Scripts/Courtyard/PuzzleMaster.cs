using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [Header("Correct Buttons (in order)")]
    public GameObject[] correctButtons;

    [Header("Required restore color per stage (after pressing correct button)")]
    // Stage 0 complete -> waiting for Blue, Stage 1 -> Red, Stage 2 -> Black
    public string[] requiredRestoreColor; // e.g. "Blue", "Red", "Black"

    private int currentStage = 0;
    private bool waitingForRestore = false;

    void Start()
    {
        // Only button 0 is active at start
        for (int i = 1; i < correctButtons.Length; i++)
            correctButtons[i].SetActive(false);
    }

    public void CorrectButtonPressed()
    {
        waitingForRestore = true;
        Debug.Log($"CorrectButtonPressed — waitingForRestore=true, stage={currentStage}, requiredColor={requiredRestoreColor[currentStage]}");
    }

    public void OnLightsRestored(string colorTag)
    {
        Debug.Log($"OnLightsRestored called — colorTag='{colorTag}', waitingForRestore={waitingForRestore}, currentStage={currentStage}, required='{requiredRestoreColor[currentStage]}'");

        if (!waitingForRestore) return;
        if (currentStage >= requiredRestoreColor.Length) return;
        if (colorTag != requiredRestoreColor[currentStage]) return;

        waitingForRestore = false;
        currentStage++;
        Debug.Log($"Stage advanced to {currentStage}");

        if (currentStage < correctButtons.Length)
        {
            correctButtons[currentStage].SetActive(true);
            Debug.Log($"Enabled correctButtons[{currentStage}]");
        }
        else
        {
            Debug.Log("Puzzle complete!");
        }
    }

    public int GetStage() => currentStage;
}