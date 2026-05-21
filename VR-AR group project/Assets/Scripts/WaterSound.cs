using UnityEngine;

public class WaterSound : MonoBehaviour
{
    public AudioClip splashSound;
    [Range(0f, 1f)]
    public float maxVolume = 0.5f;
    public float moveThreshold = 0.01f;
    public float fadeSpeed = 2f;

    private AudioSource audioSource;
    private int waterCount = 0;
    private Vector3 lastPosition;
    private bool isMoving = false;

    void Start()
    {
        lastPosition = transform.position;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = splashSound;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 0f;
        audioSource.Play();
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            audioSource.Pause();
            return;
        }
        else
        {
            audioSource.UnPause();
        }

        if (waterCount > 0)
        {
            Vector3 currentPos = transform.position;
            Vector3 lastPos = lastPosition;
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

        lastPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            waterCount++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            waterCount = Mathf.Max(0, waterCount - 1);
    }
}