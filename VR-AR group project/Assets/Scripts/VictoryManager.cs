using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip victorySound;

    [Header("Settings")]
    public float soundDuration = 5f;

    void Start()
    {
        if (audioSource && victorySound)
        {
            audioSource.PlayOneShot(victorySound);
            StartCoroutine(StopSoundAfter(soundDuration));
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private System.Collections.IEnumerator StopSoundAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (audioSource) audioSource.Stop();
    }
}