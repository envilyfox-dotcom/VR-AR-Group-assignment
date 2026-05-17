using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    [Header("Settings")]
    public float stepInterval = 0.4f;
    public float moveThreshold = 0.01f;

    private float stepTimer = 0f;
    private Vector3 lastPosition;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        lastPosition = mainCamera.position;
    }

    void Update()
    {
        Vector3 currentPosition = mainCamera.position;
        Vector3 delta = currentPosition - lastPosition;
        delta.y = 0;

        float distanceMoved = delta.magnitude;

        bool isMoving = distanceMoved > moveThreshold;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        lastPosition = currentPosition;
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int randomIndex = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[randomIndex]);

        Debug.Log($"[Footstep] Playing clip: {footstepClips[randomIndex].name}"); // ← added
    }
}