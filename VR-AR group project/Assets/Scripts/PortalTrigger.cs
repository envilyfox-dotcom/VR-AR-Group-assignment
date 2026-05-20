using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SharedResources.sceneCount = (SharedResources.sceneCount + 1) % SharedResources.sceneIndices.Length;
            SceneManager.LoadScene(SharedResources.sceneIndices[SharedResources.sceneCount]);
        }
    }
}