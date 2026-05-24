using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;

public class FloorTeleporter : MonoBehaviour
{
    [Header("Destination")]
    public Transform destination;

    [Header("Settings")]
    public float teleportDelay = 0.5f;
    public Color padActiveColor = Color.cyan;
    public Color padCooldownColor = Color.red;

    [Header("Audio")]
    public AudioClip teleportSound;
    [Range(0f, 1f)] public float volume = 1f;

    private bool isReady = true;
    private bool playerOnPad = false;
    private Renderer padRenderer;
    private Color originalColor;
    private XROrigin xrOrigin;
    private AudioSource audioSource;

    public AudioMixerGroup teleportMixerGroup;

    void Start()
    {
        padRenderer = GetComponent<Renderer>();
        if (padRenderer != null)
        {
            originalColor = padRenderer.material.color;
            padRenderer.material.color = padActiveColor;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;

        audioSource.outputAudioMixerGroup = teleportMixerGroup;

        xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin == null)
            Debug.LogError("XROrigin NOT FOUND in scene!");
        else
            Debug.Log("XROrigin found: " + xrOrigin.gameObject.name);

        if (destination == null)
            Debug.LogError("No destination set on: " + gameObject.name);
        else
            Debug.Log(gameObject.name + " destination: " + destination.name);

        Collider col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("NO COLLIDER on pad: " + gameObject.name);
        else if (!col.isTrigger)
            Debug.LogError("Collider is NOT a trigger on: " + gameObject.name);
        else
            Debug.Log("Collider OK on: " + gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered " + gameObject.name + " trigger: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Ignoring — not tagged Player. Tag was: " + other.tag);
            return;
        }

        playerOnPad = true;
        if (!isReady)
        {
            Debug.Log("Pad not ready yet");
            return;
        }

        Debug.Log("Starting teleport!");
        StartCoroutine(Teleport());
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited " + gameObject.name + ": " + other.gameObject.name);
        if (!other.CompareTag("Player")) return;
        playerOnPad = false;
    }

    IEnumerator Teleport()
    {
        isReady = false;

        if (padRenderer != null)
            padRenderer.material.color = padCooldownColor;

        yield return new WaitForSeconds(teleportDelay);

        Debug.Log("Teleporting to: " + destination.name + " at " + destination.position);

        Vector3 headOffset = Camera.main.transform.position - xrOrigin.transform.position;
        headOffset.y = 0;
        xrOrigin.transform.position = destination.position - headOffset;

        Debug.Log("XROrigin moved to: " + xrOrigin.transform.position);

        // Play sound after teleport
        if (teleportSound != null)
            audioSource.PlayOneShot(teleportSound, volume);
        else
            Debug.LogWarning("No teleport sound assigned on: " + gameObject.name);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !playerOnPad);

        if (padRenderer != null)
            padRenderer.material.color = padActiveColor;

        isReady = true;
        Debug.Log(gameObject.name + " is ready again");
    }
}