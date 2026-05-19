using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    // called when a GameObject collides with the collider
    void OnTriggerEnter(Collider other)
    {
        // check whether the player collided with the trigger
        if(other.tag == "Player")
        {
            // move to the next scene
            SharedResources.sceneCount++;
            SharedResources.sceneCount %= 2;

            // determine the scene name
            string nextScene = SharedResources.sceneName;

            if (SharedResources.sceneCount == 0 )
            {
                nextScene = SharedResources.sceneName + "b";
            }

            // load the next scene
            SceneManager.LoadScene(nextScene);
        }
    }
}
