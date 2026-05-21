// DoorController.cs
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum OpenMode { Rotate, Translate }

    [Header("Animation Settings")]
    [Tooltip("Rotate = hinge door, Translate = sliding door.")]
    public OpenMode openMode = OpenMode.Rotate;

    [Tooltip("(Rotate) Degrees to rotate around the local Y axis when opened. " +
             "Negative value swings the other way.")]
    public float openAngle = 90f;

    [Tooltip("(Translate) World-space direction and distance to slide (e.g. Vector3.up * 3).")]
    public Vector3 slideVector = Vector3.up * 3f;

    [Tooltip("Seconds the open animation takes.")]
    public float openDuration = 1.5f;

    [Tooltip("Animation curve — default gives a smooth ease-in/out.")]
    public AnimationCurve openCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Tooltip("Play this audio clip when the door starts opening (optional).")]
    public AudioClip openSound;

    private bool _isOpen = false;
    private bool _isAnimating = false;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    private AudioSource _audio;

    private void Awake()
    {
        _closedRotation = transform.localRotation;
        _openRotation = _closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        _closedPosition = transform.position;
        _openPosition = transform.position + slideVector;

        _audio = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (_isOpen || _isAnimating) return;
        StartCoroutine(AnimateDoor(opening: true));
    }

    public void CloseDoor()
    {
        if (!_isOpen || _isAnimating) return;
        StartCoroutine(AnimateDoor(opening: false));
    }

    private IEnumerator AnimateDoor(bool opening)
    {
        _isAnimating = true;

        if (openSound != null && _audio != null)
            _audio.PlayOneShot(openSound);

        Quaternion startRot = opening ? _closedRotation : _openRotation;
        Quaternion targetRot = opening ? _openRotation : _closedRotation;

        Vector3 startPos = opening ? _closedPosition : _openPosition;
        Vector3 targetPos = opening ? _openPosition : _closedPosition;

        float elapsed = 0f;
        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = openCurve.Evaluate(Mathf.Clamp01(elapsed / openDuration));

            if (openMode == OpenMode.Rotate)
                transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            else
                transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        if (openMode == OpenMode.Rotate)
            transform.localRotation = targetRot;
        else
            transform.position = targetPos;

        _isOpen = opening;
        _isAnimating = false;
    }
}
