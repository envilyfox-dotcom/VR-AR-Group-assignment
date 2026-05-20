using UnityEngine;
using UnityEngine.SceneManagement;

public static class SharedResources
{
    // 0 = MainMenu, 1 = Level 1 Darren, 2 = Workshop_Vann, 3 = regina
    public static int[] sceneIndices = { 1, 2, 3 };
    public static int sceneCount = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < sceneIndices.Length; i++)
        {
            if (sceneIndices[i] == currentBuildIndex)
            {
                sceneCount = i;
                break;
            }
        }
    }
}