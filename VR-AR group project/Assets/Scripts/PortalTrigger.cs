using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger hit by: {other.name}, tag: {other.tag}");
        if (other.CompareTag("Player"))
        {
            SharedResources.currentLevel = (SharedResources.currentLevel + 1) % SharedResources.levelIndices.Length;
            SceneManager.LoadScene(SharedResources.levelIndices[SharedResources.currentLevel]);
        }
    }
}