using UnityEngine;
using UnityEngine.SceneManagement;

public static class SharedResources
{
    public static int[] levelIndices = { 1, 2, 3 };
    public static int currentLevel = 0;

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