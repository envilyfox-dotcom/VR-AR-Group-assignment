using UnityEngine;
using TMPro;
using System.Collections;

public class TextFlicker : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float minDelay = 0.05f;
    public float maxDelay = 0.3f;
    public float minAlpha = 0.6f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float alpha = Random.Range(minAlpha, 1f);
            text.alpha = alpha;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}