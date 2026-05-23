using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject settingsPanel;
    public GameObject mainButtonsPanel;

    [Header("UI Click Sound")]
    public AudioSource uiAudioSource; // Assign an AudioSource on this GameObject
    public AudioClip buttonClickClip;

    private bool isPaused = false;

    private void Awake()
    {
        if (uiAudioSource != null)
        {
            // This AudioSource will keep playing even when AudioListener is paused
            uiAudioSource.ignoreListenerPause = true;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        menuCanvas.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        // Pause all game audio, but NOT sources with ignoreListenerPause = true
        AudioListener.pause = isPaused;

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
        Time.timeScale = 1f;
        AudioListener.pause = false;
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

    public void PlayClickSound()
    {
        if (uiAudioSource != null && buttonClickClip != null)
            uiAudioSource.PlayOneShot(buttonClickClip);
    }
}