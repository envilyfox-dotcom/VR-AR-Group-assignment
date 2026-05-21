using System.Collections;
using UnityEngine;

public class ElevatorBuilder : MonoBehaviour
{
    [Header("Elevator Pieces - assign in order")]
    public GameObject[] elevatorPieces;  // drag pieces in build order

    [Header("Settings")]
    public float pieceAppearDelay = 0.5f;
    public AudioSource buildSound;

    private bool[] piecesBuilt;
    private int totalActivated = 0;

    public static ElevatorBuilder Instance;

    void Awake()
    {
        Instance = this;
        piecesBuilt = new bool[elevatorPieces.Length];

        // hide all pieces at start
        foreach (GameObject piece in elevatorPieces)
            if (piece != null)
                piece.SetActive(false);
    }

    public void ActivateButton(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= elevatorPieces.Length)
        {
            Debug.LogError("Button index out of range: " + buttonIndex);
            return;
        }

        if (piecesBuilt[buttonIndex])
        {
            Debug.Log("Piece " + buttonIndex + " already built");
            return;
        }

        piecesBuilt[buttonIndex] = true;
        totalActivated++;
        Debug.Log("Real button activated! Building piece " + buttonIndex + ". Total: " + totalActivated + "/4");

        StartCoroutine(BuildPiece(buttonIndex));
    }

    IEnumerator BuildPiece(int index)
    {
        yield return new WaitForSeconds(pieceAppearDelay);

        if (elevatorPieces[index] != null)
        {
            elevatorPieces[index].SetActive(true);

            if (buildSound != null)
                buildSound.Play();

            Debug.Log("Piece " + index + " appeared!");
        }

        if (totalActivated >= 4)
        {
            Debug.Log("All 4 buttons activated! Elevator complete!");
            StartCoroutine(ElevatorComplete());
        }
    }

    IEnumerator ElevatorComplete()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Elevator is ready to use!");
        // hook in any extra logic here e.g. unlock elevator doors
    }
}   