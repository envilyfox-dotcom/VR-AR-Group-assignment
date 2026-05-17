using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    void Start()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        // Replace "GameScene" with your actual scene name
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("[Menu] Quitting game...");
        Application.Quit();
    }
}