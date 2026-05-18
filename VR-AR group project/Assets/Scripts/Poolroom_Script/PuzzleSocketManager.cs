using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Attach this script to a manager GameObject in your scene.
/// It listens to XRSocketInteractors and checks whether the correct
/// object has been placed in each designated puzzle socket.
///
/// SETUP STEPS:
/// 1. Create an empty GameObject called "PuzzleManager" and attach this script.
/// 2. In the Inspector, add 3 entries to "Puzzle Sockets".
///    - Socket:          drag the XRSocketInteractor on the wall
///    - Required Object: drag the specific XR Grab Interactable that must go there
///    - Label:           friendly name shown in the confirmation message (e.g. "Key", "Gem", "Rune")
/// 3. Optionally wire OnPieceCorrect / OnPuzzleComplete UnityEvents to UI, audio, etc.
/// </summary>
public class PuzzleSocketManager : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleSocket
    {
        [Tooltip("The XRSocketInteractor on the wall.")]
        public XRSocketInteractor socket;

        [Tooltip("The specific XRGrabInteractable that must be placed here.")]
        public XRGrabInteractable requiredObject;

        [Tooltip("Friendly label used in confirmation messages.")]
        public string label = "Piece";

        [HideInInspector] public bool isSolved = false;
    }

    [Header("Puzzle Configuration")]
    [Tooltip("Define each socket and its required object. Add exactly 3 for a 1/3 → 2/3 → complete flow.")]
    public List<PuzzleSocket> puzzleSockets = new List<PuzzleSocket>();

    [Header("Events")]
    [Tooltip("Fired when a correct piece is placed. Passes the progress string, e.g. '1/3'.")]
    public UnityEvent<string> OnPieceCorrect;

    [Tooltip("Fired when a wrong piece is placed in a socket.")]
    public UnityEvent<string> OnPieceIncorrect;

    [Tooltip("Fired once when all puzzle pieces are correctly placed.")]
    public UnityEvent OnPuzzleComplete;

    // -----------------------------------------------------------------------
    private int _solvedCount = 0;

    private void OnEnable()
    {
        foreach (var ps in puzzleSockets)
        {
            if (ps.socket == null) continue;
            // Cache the puzzle socket reference so the lambda can close over it.
            var captured = ps;
            ps.socket.selectEntered.AddListener(_ => OnObjectPlaced(captured));
            ps.socket.selectExited.AddListener(_ => OnObjectRemoved(captured));
        }
    }

    private void OnDisable()
    {
        foreach (var ps in puzzleSockets)
        {
            if (ps.socket == null) continue;
            var captured = ps;
            ps.socket.selectEntered.RemoveAllListeners();
            ps.socket.selectExited.RemoveAllListeners();
        }
    }

    // -----------------------------------------------------------------------
    private void OnObjectPlaced(PuzzleSocket ps)
    {
        // XRSocketInteractor.GetOldestInteractableSelected() returns what just landed.
        var placed = ps.socket.GetOldestInteractableSelected() as XRGrabInteractable;

        if (placed == null) return;

        if (placed == ps.requiredObject)
        {
            if (!ps.isSolved)
            {
                ps.isSolved = true;
                _solvedCount++;

                string progress = $"{_solvedCount}/{puzzleSockets.Count}";
                string msg = $"✔ Correct! \"{ps.label}\" placed — {progress} of the puzzle complete.";

                Debug.Log(msg);
                OnPieceCorrect?.Invoke(progress);

                if (_solvedCount >= puzzleSockets.Count)
                {
                    Debug.Log("🎉 Puzzle fully solved!");
                    OnPuzzleComplete?.Invoke();
                }
            }
        }
        else
        {
            string msg = $"✘ Wrong object in \"{ps.label}\" socket.";
            Debug.Log(msg);
            OnPieceIncorrect?.Invoke(msg);
        }
    }

    private void OnObjectRemoved(PuzzleSocket ps)
    {
        // If the correct piece is pulled back out, un-solve that slot.
        if (ps.isSolved)
        {
            ps.isSolved = false;
            _solvedCount = Mathf.Max(0, _solvedCount - 1);
            Debug.Log($"↩ \"{ps.label}\" piece removed — puzzle reset to {_solvedCount}/{puzzleSockets.Count}.");
        }
    }

    // -----------------------------------------------------------------------
    /// <summary>Call from UI button or script to reset all puzzle state.</summary>
    public void ResetPuzzle()
    {
        foreach (var ps in puzzleSockets) ps.isSolved = false;
        _solvedCount = 0;
        Debug.Log("Puzzle reset.");
    }
}