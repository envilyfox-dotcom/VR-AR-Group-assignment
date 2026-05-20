using UnityEngine;
using UnityEngine.SceneManagement;

public static class SharedResources
{
    // Your 3 main levels from Build Settings: 1, 2, 3
    public static int[] levelIndices = { 1, 2, 3 };
    public static int currentLevel = 0;

    // Automatically figure out which level we're on when a scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < levelIndices.Length; i++)
        {
            if (levelIndices[i] == currentBuildIndex)
            {
                currentLevel = i;
                break;
            }
        }
    }
}