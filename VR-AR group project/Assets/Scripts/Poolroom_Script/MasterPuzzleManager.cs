using UnityEngine;
using UnityEngine.Events;

public class MasterPuzzleManager : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Fired once when both puzzles have called NotifyPuzzleSolved().")]
    public UnityEvent OnBothPuzzlesSolved;

    [Tooltip("Optional — fired when the first puzzle is solved.")]
    public UnityEvent OnFirstPuzzleSolved;

    private int _solvedCount = 0;
    private bool _allSolved = false;

    public void NotifyPuzzleSolved()
    {
        if (_allSolved) return;

        _solvedCount++;
        Debug.Log($"[Master] Puzzle {_solvedCount}/2 solved.");

        if (_solvedCount == 1)
        {
            OnFirstPuzzleSolved?.Invoke();
        }

        if (_solvedCount >= 2)
        {
            _allSolved = true;
            Debug.Log("[Master] Both puzzles solved — opening the door!");
            OnBothPuzzlesSolved?.Invoke();
        }
    }

    public void ResetAll()
    {
        _solvedCount = 0;
        _allSolved = false;
        Debug.Log("[Master] Reset.");
    }
}