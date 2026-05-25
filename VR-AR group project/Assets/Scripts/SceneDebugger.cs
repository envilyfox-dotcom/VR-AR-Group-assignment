using UnityEngine;
using UnityEngine.InputSystem;

public class SceneDebugger : MonoBehaviour
{
    void Start()
    {
        foreach (var actionMap in InputSystem.ListEnabledActions())
        {
            Debug.Log("Active action: " + actionMap.name + " | Map: " + actionMap.actionMap.name);
        }
    }
}