using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        SpawnPoint spawnPoint = FindFirstObjectByType<SpawnPoint>();

        if (spawnPoint != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Spawning at: " + spawnPoint.transform.position);
        }
        else
        {
            Debug.LogWarning("No SpawnPoint found in scene!");
        }
    }
}