// PortalTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Always derive currentLevel from the actual loaded scene,
        // don't trust the static value
        int resolvedLevel = -1;
        for (int i = 0; i < SharedResources.levelIndices.Length; i++)
        {
            if (SharedResources.levelIndices[i] == currentBuildIndex)
            {
                resolvedLevel = i;
                break;
            }
        }

        if (resolvedLevel == -1)
        {
            Debug.LogWarning($"PortalTrigger: Build index {currentBuildIndex} not in levelIndices!");
            return;
        }

        int nextLevel = (resolvedLevel + 1) % SharedResources.levelIndices.Length;
        SharedResources.currentLevel = nextLevel;

        Debug.Log($"PortalTrigger: {currentBuildIndex} → level index {nextLevel} → build index {SharedResources.levelIndices[nextLevel]}");
        SceneManager.LoadScene(SharedResources.levelIndices[nextLevel]);
    }
}