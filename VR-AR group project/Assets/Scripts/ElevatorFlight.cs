using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class ElevatorFlight : MonoBehaviour
{
    [Header("References")]
    public GameObject[] elevatorPieces;   // same pieces as ElevatorBuilder
    public GameObject ledge;              // the safety ledge object
    public GameObject flightButton;       // the button inside elevator

    [Header("Flight Settings")]
    public float targetY = 50f;           // how high to fly
    public float flightSpeed = 5f;
    public float ledgeDelay = 0.3f;       // ledge appears slightly after button press

    private XROrigin xrOrigin;
    private bool isFlying = false;
    private bool hasFlown = false;
    private XRBaseInteractable buttonInteractable;

    void Start()
    {
        xrOrigin = FindFirstObjectByType<XROrigin>();

        // hide ledge at start
        if (ledge != null)
            ledge.SetActive(false);

        // hide flight button until elevator is built
        if (flightButton != null)
            flightButton.SetActive(false);

        buttonInteractable = flightButton.GetComponent<XRBaseInteractable>();
        if (buttonInteractable != null)
            buttonInteractable.selectEntered.AddListener(OnFlightButtonPressed);
        else
            Debug.LogError("No XR Interactable on flight button!");
    }

    // called by ElevatorBuilder when all 4 pieces are built
    public void OnElevatorComplete()
    {
        Debug.Log("Elevator complete — flight button activated!");
        if (flightButton != null)
            flightButton.SetActive(true);
    }

    void OnFlightButtonPressed(SelectEnterEventArgs args)
    {
        if (hasFlown || isFlying) return;
        Debug.Log("Flight button pressed! Elevator launching!");
        StartCoroutine(LaunchElevator());
    }

    IEnumerator LaunchElevator()
    {
        isFlying = true;
        hasFlown = true;

        // disable the button immediately
        if (buttonInteractable != null)
            buttonInteractable.enabled = false;

        // show ledge after short delay
        yield return new WaitForSeconds(ledgeDelay);
        if (ledge != null)
        {
            ledge.SetActive(true);
            Debug.Log("Safety ledge appeared!");
        }

        // fly elevator pieces + player up together
        float startY = elevatorPieces[0].transform.position.y;
        Debug.Log("Flying from Y: " + startY + " to Y: " + targetY);

        while (true)
        {
            float currentY = elevatorPieces[0].transform.position.y;

            if (currentY >= targetY)
            {
                // snap everything exactly to target
                float diff = targetY - currentY;
                MoveEverything(diff);
                Debug.Log("Reached target Y: " + targetY);
                break;
            }

            float moveAmount = flightSpeed * Time.deltaTime;
            MoveEverything(moveAmount);
            yield return null;
        }

        isFlying = false;
        Debug.Log("Elevator stopped!");
    }

    void MoveEverything(float amount)
    {
        // move all elevator pieces
        foreach (GameObject piece in elevatorPieces)
            if (piece != null)
                piece.transform.position += Vector3.up * amount;

        // move ledge with elevator
        if (ledge != null)
            ledge.transform.position += Vector3.up * amount;

        // move flight button with elevator
        if (flightButton != null)
            flightButton.transform.position += Vector3.up * amount;

        // move player with elevator
        if (xrOrigin != null)
            xrOrigin.transform.position += Vector3.up * amount;
    }

    void OnDestroy()
    {
        if (buttonInteractable != null)
            buttonInteractable.selectEntered.RemoveListener(OnFlightButtonPressed);
    }
}