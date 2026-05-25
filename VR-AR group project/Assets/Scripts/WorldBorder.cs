using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    private XROriginSpawner xrOriginSpawner;
    private SpawnPoint spawnPoint;

    void Start()
    {
        xrOriginSpawner = FindFirstObjectByType<XROriginSpawner>();
        spawnPoint = FindFirstObjectByType<SpawnPoint>();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (xrOriginSpawner != null && spawnPoint != null)
        {
            Camera cam = Camera.main;
            xrOriginSpawner.PlaceAtSpawn(cam.transform, spawnPoint.transform);
            Debug.Log("WorldBorder: Player out of bounds, teleporting to spawn.");
        }
    }
}