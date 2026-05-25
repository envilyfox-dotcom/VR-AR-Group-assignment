using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleIndicatorUI : MonoBehaviour
{
    [Header("Text")]
    [Tooltip("Displays '1/3', '2/3', '3/3'")]
    public TextMeshProUGUI progressText;

    [Tooltip("Displays 'Correct!', 'Wrong piece', 'Puzzle solved!'")]
    public TextMeshProUGUI feedbackText;

    [Header("Progress Bar (optional)")]
    [Tooltip("Image set to Filled / Horizontal. Its fillAmount tracks progress.")]
    public Image progressBar;

    [Header("Colors")]
    public Color correctColor = new Color(0.2f, 0.85f, 0.4f);
    public Color incorrectColor = new Color(0.95f, 0.3f, 0.25f);
    public Color completeColor = new Color(1f, 0.85f, 0.1f);

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
        if (progressBar) progressBar.fillAmount = 0f;
        if (feedbackText) feedbackText.alpha = 0f;
    }

    public void ShowProgress(string progress)
    {
        if (progressText) progressText.text = progress;

        var parts = progress.Split('/');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int solved) &&
            int.TryParse(parts[1], out int total))
        {
            if (progressBar) progressBar.fillAmount = (float)solved / total;
        }
    }

    public void ShowWrong(string msg = "Wrong piece!")
    {
        ShowFeedback("✘  " + msg, incorrectColor);
    }

    public void ShowComplete()
    {
        ShowFeedback("🎉  Puzzle solved!", completeColor);
        if (progressText) progressText.color = completeColor;
    }

    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        if (_feedbackRoutine != null) StopCoroutine(_feedbackRoutine);
        _feedbackRoutine = StartCoroutine(FeedbackRoutine(message, color));
    }

    private IEnumerator FeedbackRoutine(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.alpha = 1f;

        // Hold
        yield return new WaitForSeconds(feedbackDuration);

        // Fade out
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