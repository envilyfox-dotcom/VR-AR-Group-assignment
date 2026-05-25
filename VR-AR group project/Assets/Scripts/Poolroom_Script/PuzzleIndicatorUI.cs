using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleIndicatorUI : MonoBehaviour
{
    [Header("Text")]
    [Tooltip("Displays '1/3', '2/3', '3/3'")]
    public TextMeshProUGUI progressText;

    [Tooltip("Displays 'Correct!', 'Wrong piece', 'Puzzle solved!'")]
    public TextMeshProUGUI feedbackText;

    [Header("Timing")]
    [Tooltip("How long the feedback text stays visible before fading.")]
    public float feedbackDuration = 2.5f;

    private int _total;
    private Coroutine _feedbackRoutine;

    private void Start()
    {
        var mgr = FindFirstObjectByType<PuzzleSocketManager>();
        _total = mgr != null ? mgr.puzzleSockets.Count : 3;

        if (progressText) progressText.text = $"0/{_total}";
        if (feedbackText) feedbackText.alpha = 0f;
    }

    public void ShowProgress(string progress)
    {
        if (progressText) progressText.text = progress;
    }

    public void ShowWrong(string msg = "Wrong piece!")
    {
        ShowFeedback("✘  " + msg);
    }

    public void ShowComplete()
    {
        ShowFeedback("🎉  Puzzle solved!");
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        if (_feedbackRoutine != null) StopCoroutine(_feedbackRoutine);
        _feedbackRoutine = StartCoroutine(FeedbackRoutine(message));
    }

    private IEnumerator FeedbackRoutine(string message)
    {
        feedbackText.text = message;
        feedbackText.alpha = 1f;

        yield return new WaitForSeconds(feedbackDuration);

        float t = 0f;
        float fadeTime = 0.5f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            feedbackText.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }
        feedbackText.alpha = 0f;
    }
}