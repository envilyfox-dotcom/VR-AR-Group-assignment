using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ClockPuzzleManager : MonoBehaviour
{
    [Header("Arrow Socket References")]
    public XRSocketInteractor hourSocket;
    public XRSocketInteractor minuteSocket;

    [Header("Target Rotations (degrees around Z axis)")]
    public float hourTargetAngle = 270f;    // 9 o'clock
    public float minuteTargetAngle = 0f;    // 12 o'clock

    [Header("Tolerance")]
    public float angleTolerance = 10f;

    [Header("Events")]
    [Tooltip("Fired when the hour hand is correct.")]
    public UnityEvent OnHourCorrect;

    [Tooltip("Fired when the hour hand becomes incorrect (e.g. player rotates it away or removes it).")]
    public UnityEvent OnHourIncorrect;

    [Tooltip("Fired when the minute hand is correct.")]
    public UnityEvent OnMinuteCorrect;

    [Tooltip("Fired when the minute hand becomes incorrect.")]
    public UnityEvent OnMinuteIncorrect;

    [Tooltip("Fired once when both hands are correct and the puzzle is solved.")]
    public UnityEvent OnPuzzleSolved;

    [Header("State")]
    [SerializeField] private bool isSolved = false;
    [SerializeField] private bool wasHourCorrect = false;
    [SerializeField] private bool wasMinuteCorrect = false;

    void Update()
    {
        if (isSolved) return;

        bool hourCorrect = CheckSocket(hourSocket, ArrowType.Type.Hour, hourTargetAngle);
        bool minuteCorrect = CheckSocket(minuteSocket, ArrowType.Type.Minute, minuteTargetAngle);

        if (hourCorrect != wasHourCorrect)
        {
            wasHourCorrect = hourCorrect;
            if (hourCorrect)
            {
                Debug.Log("Hour hand is correct!");
                OnHourCorrect?.Invoke();
            }
            else
            {
                Debug.Log("Hour hand is no longer correct.");
                OnHourIncorrect?.Invoke();
            }
        }

        if (minuteCorrect != wasMinuteCorrect)
        {
            wasMinuteCorrect = minuteCorrect;
            if (minuteCorrect)
            {
                Debug.Log("Minute hand is correct!");
                OnMinuteCorrect?.Invoke();
            }
            else
            {
                Debug.Log("Minute hand is no longer correct.");
                OnMinuteIncorrect?.Invoke();
            }
        }

        if (hourCorrect && minuteCorrect)
        {
            isSolved = true;
            Debug.Log("Clock puzzle solved! It's 9:00 AM!");
            OnPuzzleSolved?.Invoke();
        }
    }

    private bool CheckSocket(XRSocketInteractor socket, ArrowType.Type expectedType, float targetAngle)
    {
        if (!IsSocketOccupied(socket)) return false;
        if (!IsCorrectArrowType(socket, expectedType)) return false;

        float angle = GetSocketAttachAngle(socket);
        return AngleMatches(angle, targetAngle);
    }

    private bool IsSocketOccupied(XRSocketInteractor socket)
    {
        if (socket == null) return false;
        return socket.interactablesSelected.Count > 0;
    }

    private bool IsCorrectArrowType(XRSocketInteractor socket, ArrowType.Type expectedType)
    {
        var interactable = socket.interactablesSelected[0];
        var arrow = interactable.transform.GetComponent<ArrowType>();
        return arrow != null && arrow.arrowType == expectedType;
    }

    private float GetSocketAttachAngle(XRSocketInteractor socket)
    {
        Transform attach = socket.attachTransform != null
            ? socket.attachTransform
            : socket.transform;

        float angle = attach.rotation.eulerAngles.z;
        return (angle % 360f + 360f) % 360f;
    }

    private bool AngleMatches(float current, float target)
    {
        float diff = Mathf.Abs(Mathf.DeltaAngle(current, target));
        return diff <= angleTolerance;
    }

    public void ResetPuzzle()
    {
        isSolved = false;
        wasHourCorrect = false;
        wasMinuteCorrect = false;
        Debug.Log("Clock puzzle reset.");
    }
}