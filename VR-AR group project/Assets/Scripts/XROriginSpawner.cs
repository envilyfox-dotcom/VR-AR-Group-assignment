using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class XROriginSpawner : MonoBehaviour
{
    [Tooltip("Max seconds to wait for camera before placing anyway")]
    [SerializeField] private float _timeoutSeconds = 15f;

    [Tooltip("How many stable frames to wait before placing")]
    [SerializeField] private int _requiredStableFrames = 3;

    [Tooltip("Max camera drift per frame to be considered stable")]
    [SerializeField] private float _stabilityThreshold = 0.002f;

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => StartCoroutine(PlaceXROrigin());

    IEnumerator PlaceXROrigin()
    {
        Transform camera = null;
        float elapsed = 0f;
        while (camera == null)
        {
            camera = Camera.main?.transform;
            elapsed += Time.deltaTime;
            if (elapsed > _timeoutSeconds)
            {
                Debug.LogError("XROriginSpawner: Camera never found!");
                yield break;
            }
            yield return null;
        }

        Vector3 prevPos = camera.position;
        int stableFrames = 0;
        elapsed = 0f;

        while (stableFrames < _requiredStableFrames)
        {
            yield return null;
            elapsed += Time.deltaTime;

            float drift = Vector3.Distance(camera.position, prevPos);
            stableFrames = drift < _stabilityThreshold ? stableFrames + 1 : 0;
            prevPos = camera.position;

            if (elapsed >= _timeoutSeconds)
            {
                Debug.LogWarning("XROriginSpawner: Timed out, placing anyway.");
                break;
            }
        }

        SpawnPoint spawnPoint = FindFirstObjectByType<SpawnPoint>();
        if (spawnPoint == null)
        {
            Debug.LogError("XROriginSpawner: No SpawnPoint found!");
            yield break;
        }

        PlaceAtSpawn(camera, spawnPoint.transform);
    }

    public void PlaceAtSpawn(Transform camera, Transform spawnPoint)
    {
        Vector3 cameraOffsetFromOrigin = camera.position - transform.position;

        Vector3 horizontalOffset = new Vector3(cameraOffsetFromOrigin.x, 0f, cameraOffsetFromOrigin.z);

        transform.position = new Vector3(
            spawnPoint.position.x - horizontalOffset.x,
            spawnPoint.position.y,
            spawnPoint.position.z - horizontalOffset.z
        );

        // Rotate the rig so the camera faces the spawn point's forward direction
        float yawDiff = spawnPoint.eulerAngles.y - camera.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + yawDiff, 0f);

        Debug.Log($"XROriginSpawner: Camera placed at {camera.position} facing {camera.eulerAngles.y}°");
    }
}