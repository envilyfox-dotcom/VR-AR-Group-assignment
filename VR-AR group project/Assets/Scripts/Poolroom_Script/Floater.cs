using UnityEngine;

public class Floater : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float _waterSurfaceY = 0f;

    [Header("Bob Settings")]
    [SerializeField] private float _submersionDepth = 0f;
    [SerializeField] private float _bobSpeed = 1f;
    [SerializeField] private float _bobHeight = 0.1f;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationAmount = 5f;

    [Header("Sync")]
    [SerializeField] private bool _randomizeOffset = true;

    private float _timeOffset;
    private Vector3 _startRotation;

    void Start()
    {
        _timeOffset = _randomizeOffset ? Random.Range(0f, Mathf.PI * 2f) : 0f;
        _startRotation = transform.eulerAngles;
    }

    void Update()
    {
        float wave = Mathf.Sin(Time.time * _bobSpeed + _timeOffset);

        // Bob up and down
        float targetY = _waterSurfaceY - _submersionDepth + (wave * _bobHeight);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

        // Rock side to side
        float tilt = wave * _rotationAmount;
        transform.rotation = Quaternion.Euler(
            _startRotation.x + tilt,
            _startRotation.y,
            _startRotation.z + tilt * 0.5f
        );
    }
}