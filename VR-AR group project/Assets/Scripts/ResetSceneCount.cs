using UnityEngine;

public class ResetSceneCount : MonoBehaviour
{
    void Start()
    {
        SharedResources.sceneCount = 0;
    }
}