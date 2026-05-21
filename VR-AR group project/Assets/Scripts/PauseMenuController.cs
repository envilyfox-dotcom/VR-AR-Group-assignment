using UnityEngine;
using UnityEngine.SceneManagement;

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
        Time.timeScale = isPaused ? 0f : 1f;

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

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Must reset before loading or the scene starts frozen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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