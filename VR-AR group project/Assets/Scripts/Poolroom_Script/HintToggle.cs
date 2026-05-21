using UnityEngine;
using UnityEngine.EventSystems;

public class HintToggle : MonoBehaviour
{
    public GameObject hintPanel;

    public void ToggleHint()
    {
        hintPanel.SetActive(!hintPanel.activeSelf);

        EventSystem.current.SetSelectedGameObject(null);
    }
}