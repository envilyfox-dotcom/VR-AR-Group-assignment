using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class XROriginSpawner : MonoBehaviour
{
    [Tooltip("How many consecutive stable frames before we consider tracking ready")]
    [SerializeField] private int _requiredStableFrames = 5;

    [Tooltip("Max camera drift per frame (meters) to be considered stable")]
    [SerializeField] private float _stabilityThreshold = 0.001f;

    [Tooltip("Max seconds to wait for stability before placing anyway")]
    [SerializeField] private float _timeoutSeconds = 15f;

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PlaceXROrigin());
    }

    IEnumerator PlaceXROrigin()
    {
        // --- STEP 1: Wait for Camera.main to exist ---
        Transform camera = null;
        while (camera == null)
        {
            camera = Camera.main?.transform;
            yield return null;
        }

        // --- STEP 2: Wait for XR tracking to stabilize ---
        Vector3 previousCamPos = camera.position;
        Vector3 positionSum = Vector3.zero;
        int stableFrameCount = 0;
        float elapsed = 0f;

        while (stableFrameCount < _requiredStableFrames)
        {
            yield return null;
            elapsed += Time.deltaTime;

            float drift = Vector3.Distance(camera.position, previousCamPos);

            if (drift < _stabilityThreshold)
            {
                stableFrameCount++;
                positionSum += camera.position; // accumulate while stable
            }
            else
            {
                stableFrameCount = 0;
                positionSum = Vector3.zero; // reset accumulation
            }

            previousCamPos = camera.position;

            if (elapsed >= _timeoutSeconds)
            {
                Debug.LogWarning($"XROriginSpawner: Timed out after {_timeoutSeconds}s — camera pos: {camera.position}, stable frames: {stableFrameCount}");
                yield return new WaitForSeconds(0.1f);
                break;
            }
        }

        // Use averaged position instead of instantaneous
        Vector3 averagedCamPos = stableFrameCount > 0 ? positionSum / stableFrameCount : camera.position;
        Vector3 offset = averagedCamPos - transform.position;

        // --- STEP 3: Find spawn point ---
        SpawnPoint spawnPoint = FindFirstObjectByType<SpawnPoint>();
        if (spawnPoint == null)
        {
            Debug.LogError("XROriginSpawner: No SpawnPoint found in scene!");
            yield break;
        }

        // --- STEP 4: Position ---

        // If camera offset is suspiciously large, tracking data is bad — skip offset
        if (Mathf.Abs(offset.x) > 10f || Mathf.Abs(offset.z) > 10f)
        {
            Debug.LogWarning($"XROriginSpawner: Camera offset too large ({offset}), ignoring — placing directly at spawn point.");
            transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
        }
        else
        {
            Vector3 newOriginPos = spawnPoint.transform.position;
            newOriginPos.x -= offset.x;
            newOriginPos.z -= offset.z;
            newOriginPos.y = spawnPoint.transform.position.y;
            transform.position = newOriginPos;
        }

        // --- STEP 5: Rotation ---
        float yawDiff = spawnPoint.transform.eulerAngles.y - camera.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + yawDiff, 0f);

        Debug.Log($"XROriginSpawner: Placed at {transform.position}. Camera at {camera.position}, yaw: {camera.eulerAngles.y}");
    }
}