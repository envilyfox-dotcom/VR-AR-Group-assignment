using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject settingsPanel;
    public GameObject mainButtonsPanel;

    [Header("UI Click Sound")]
    public AudioSource uiAudioSource;
    public AudioClip buttonClickClip;

    private bool isPaused = false;

    private void Awake()
    {
        if (uiAudioSource != null)
            uiAudioSource.ignoreListenerPause = true;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        menuCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;

        if (!isPaused)
        {
            settingsPanel.SetActive(false);
            mainButtonsPanel.SetActive(true);
        }
    }

    public void Resume()
    {
        PlayClickSound();
        isPaused = false;
        menuCanvas.SetActive(false);
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void OpenSettings()
    {
        PlayClickSound();
        settingsPanel.SetActive(true);
        mainButtonsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        PlayClickSound();
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
    }

    public void RetryLevel()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayClickSound()
    {
        if (uiAudioSource != null && buttonClickClip != null)
            uiAudioSource.PlayOneShot(buttonClickClip);
    }
}