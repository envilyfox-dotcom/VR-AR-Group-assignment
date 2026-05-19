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
        SceneManager.LoadScene("Level 1 Darren");
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}