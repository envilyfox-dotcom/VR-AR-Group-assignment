using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cycle to the next level: 0 -> 1 -> 2 -> 0
            SharedResources.currentLevel = (SharedResources.currentLevel + 1) % SharedResources.levelIndices.Length;

            int nextSceneIndex = SharedResources.levelIndices[SharedResources.currentLevel];
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}