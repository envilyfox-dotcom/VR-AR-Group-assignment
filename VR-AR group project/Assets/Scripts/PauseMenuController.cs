using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject settingsPanel;
    public GameObject mainButtonsPanel;

    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;
        menuCanvas.SetActive(isPaused);

        if (!isPaused)
        {
            settingsPanel.SetActive(false);
            mainButtonsPanel.SetActive(true);
        }
    }

    public void Resume()
    {
        isPaused = false;
        menuCanvas.SetActive(false);
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainButtonsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}