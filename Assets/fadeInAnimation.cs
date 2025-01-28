using System.Collections;
using UnityEngine;

public class fadeInAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Reference to the CanvasGroup component
    public float fadeDuration = 1.5f; // Duration for fade-in or fade-out

    void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not assigned!");
            return;
        }

        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        // Fade In
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Wait for 3 seconds (including fade duration)
        yield return new WaitForSeconds(3f - fadeDuration * 2);

        // Fade Out
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
