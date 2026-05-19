using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class RotatableSocketObject : MonoBehaviour
{
    [Header("Input (New Input System)")]
    [Tooltip("Assign your Input Action here, or leave empty to auto-detect keyboard T")]
    public InputActionReference rotateActionReference;

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationStep = 90f;
    public bool smoothRotation = true;
    public float rotationSpeed = 360f;

    [Header("State")]
    [SerializeField] private bool isInSocket = false;
    [SerializeField] private bool isGrabbed = false;

    private XRGrabInteractable grabInteractable;
    private XRSocketInteractor currentSocket = null;

    private Quaternion targetRotation;
    private bool isRotating = false;
    private InputAction rotateAction;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        targetRotation = transform.rotation;

        if (rotateActionReference != null)
            rotateAction = rotateActionReference.action;
        else
            rotateAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/t");

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void OnEnable() => rotateAction?.Enable();
    void OnDisable() => rotateAction?.Disable();

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
        if (rotateActionReference == null)
            rotateAction?.Dispose();
    }

    void Update()
    {
        if (rotateAction != null && rotateAction.WasPressedThisFrame())
        {
            if ((isInSocket || isGrabbed) && SelectedObjectManager.Current == this)
                RotateObject();
        }

        // Only smooth-rotate the object directly when grabbed (not socketed)
        if (smoothRotation && isRotating && !isInSocket)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void RotateObject()
    {
        if (isInSocket && currentSocket != null)
        {
            Transform attach = currentSocket.attachTransform != null
                ? currentSocket.attachTransform
                : currentSocket.transform;

            if (smoothRotation)
                StartCoroutine(SmoothRotateAttach(attach));
            else
                attach.rotation *= Quaternion.Euler(rotationAxis * rotationStep);
        }
        else if (isGrabbed)
        {
            targetRotation = transform.rotation * Quaternion.Euler(rotationAxis * rotationStep);
            if (smoothRotation)
                isRotating = true;
            else
                transform.rotation = targetRotation;
        }

        Debug.Log($"[{name}] Rotated {rotationStep}°. InSocket: {isInSocket}, Grabbed: {isGrabbed}");
    }

    private System.Collections.IEnumerator SmoothRotateAttach(Transform attach)
    {
        Quaternion startRot = attach.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(rotationAxis * rotationStep);

        while (Quaternion.Angle(attach.rotation, endRot) > 0.01f)
        {
            attach.rotation = Quaternion.RotateTowards(
                attach.rotation,
                endRot,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        attach.rotation = endRot;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var socket = args.interactorObject.transform.GetComponent<XRSocketInteractor>();
        if (socket != null)
        {
            isInSocket = true;
            currentSocket = socket;
        }
        else
        {
            isGrabbed = true;
        }

        SelectedObjectManager.Select(this);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        var socket = args.interactorObject.transform.GetComponent<XRSocketInteractor>();
        if (socket != null)
        {
            isInSocket = false;
            currentSocket = null;
        }
        else
        {
            isGrabbed = false;
        }

        // Only deselect if not still in a socket — keep focus while socketed
        if (!isInSocket)
            SelectedObjectManager.Deselect(this);
    }
}