using UnityEngine;
using UnityEngine.EventSystems;

public class HintToggle : MonoBehaviour
{
    public GameObject hintPanel;

    public void ToggleHint()
    {
        Debug.Log("ToggleHint called, hintPanel: " + hintPanel);

        hintPanel.SetActive(!hintPanel.activeSelf);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}