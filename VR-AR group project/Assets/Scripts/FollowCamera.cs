using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform vrCamera;         // drag your VR camera here
    public float distance = 1.5f;
    public Vector3 offset = new Vector3(0, 0.2f, 0);  // shift up slightly

    void LateUpdate()
    {
        if (!vrCamera) return;

        // position it in front of wherever the player is looking
        transform.position = vrCamera.position
                           + vrCamera.forward * distance
                           + offset;

        // face the player
        transform.rotation = vrCamera.rotation;
    }
}