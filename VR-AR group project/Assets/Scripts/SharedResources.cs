public static class SharedResources
{
<<<<<<< HEAD
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
=======
    // 0 = SampleScene, 1 = MainMenu, 2 = Level 1 Darren, 3 = Workshop_Vann, 4 = regina
    public static int[] sceneIndices = { 2, 3, 4 };
    public static int sceneCount = 0;
>>>>>>> parent of e1504a0 (trying to get the teleporter to work)
}