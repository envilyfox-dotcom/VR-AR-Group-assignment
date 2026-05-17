using UnityEngine;

public class WaterSound : MonoBehaviour
{
    public AudioClip splashSound;
    [Range(0f, 1f)]
    public float maxVolume = 0.5f;
    public float moveThreshold = 0.01f;
    public float fadeSpeed = 2f;

    private AudioSource audioSource;
    private int waterCount = 0; // counts how many water triggers we're inside
    private Vector3 lastCameraPosition;
    private Transform cameraTransform;
    private bool isMoving = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = splashSound;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 0f;
        audioSource.Play();
    }

    void Update()
    {
        if (waterCount > 0)
        {
            Vector3 currentPos = cameraTransform.position;
            Vector3 lastPos = lastCameraPosition;
            currentPos.y = 0;
            lastPos.y = 0;

            float speed = Vector3.Distance(currentPos, lastPos) / Time.deltaTime;
            isMoving = speed > moveThreshold;
        }
        else
        {
            isMoving = false;
        }

        float targetVolume = isMoving ? maxVolume : 0f;
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);

        lastCameraPosition = cameraTransform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            waterCount++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            waterCount = Mathf.Max(0, waterCount - 1); // never go below 0
    }
}