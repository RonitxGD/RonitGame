using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreMng : MonoBehaviour
{

    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private int maxScore = 1000;

    private bool abilityReady = false;
    private bool isAbilityOnCooldown = false;
    private bool isAnimating = false;
    private float abilityCooldownTime = 10f;

    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;

    public ParticleSystem abilityEffectPrefab;
    public Transform effectSpawnPoint;

    [SerializeField] private Image abilityImage;
    private AudioSource audioSource;
    public AudioClip abilityActivatedSound;

    private int currentScore;

    [SerializeField] private HigscoreMng highScoreManager;

    public void IncreaseScore(int increment)
    {
        currentScore += increment;
        score += increment;

        if (scoreSlider != null)
        {
            scoreSlider.value = score;
        }

        UpdateScoreText();
    }

    public int GetCurrentScore() => currentScore;

    public void HandlePlayerDeath()
    {
        // Pause the game
        Time.timeScale = 0;

        // Show the high score text
        if (highScoreManager != null)
        {
            highScoreManager.highScoreText.gameObject.SetActive(true);
            highScoreManager.highScoreText.text = "High Score: " + highScoreManager.GetHighScore().ToString();
        }
    }

    private void DisplayHighScoreOnDeath()
{
    if (highScoreManager != null && highScoreManager.highScoreText != null)
    {
        // Enable the high score text UI and display the high score
        highScoreManager.highScoreText.gameObject.SetActive(true);
        highScoreManager.highScoreText.text = "High Score: " + highScoreManager.GetHighScore().ToString();
    }
}

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (scoreSlider != null)
        {
            scoreSlider.maxValue = maxScore;
            scoreSlider.value = score;
        }

        if (highScoreManager == null)
        {
            highScoreManager = FindObjectOfType<HigscoreMng>();
            if (highScoreManager == null)
            {
                Debug.LogError("HighScoreManager not found in the scene.");
            }
        }
    }

    private void Update()
    {
        UpdateScoreText();

        if (abilityReady && !isAbilityOnCooldown && Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseAbility();
        }

        if (score >= maxScore && !abilityReady)
        {
            abilityReady = true;
            if (!isAnimating)
            {
                StartImageAnimation();
            }
        }
        else if (score < maxScore && abilityReady)
        {
            abilityReady = false;
            StopImageAnimation();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextMesh != null)
        {
            scoreTextMesh.text = isAbilityOnCooldown ? "I’m Invincible!" : $"Get Invincible: {score}";
        }
    }

    private void UseAbility()
    {
        Debug.Log("Ability activated!");
        abilityReady = false;

        score = Mathf.Max(score - 1000, 0);

        if (scoreSlider != null)
        {
            scoreSlider.value = score;
        }

        UpdateScoreText();

        playerHealth?.Heal(1500);
        playerHealth?.SetInvulnerable(true);

        playerMovement?.ActivateSpeedBoost();

        if (abilityEffectPrefab != null && effectSpawnPoint != null)
        {
            StartCoroutine(PlayParticleEffectForDuration(10f));
        }

        if (audioSource != null && abilityActivatedSound != null)
        {
            audioSource.PlayOneShot(abilityActivatedSound);
        }

        StartCoroutine(AbilityCooldown());
    }

    private IEnumerator AbilityCooldown()
    {
        isAbilityOnCooldown = true;
        UpdateScoreText();

        yield return new WaitForSeconds(abilityCooldownTime);

        abilityReady = true;
        isAbilityOnCooldown = false;
        UpdateScoreText();
    }

    private IEnumerator PlayParticleEffectForDuration(float duration)
    {
        ParticleSystem effectInstance = Instantiate(abilityEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        effectInstance.transform.SetParent(effectSpawnPoint);
        effectInstance.Play();

        yield return new WaitForSeconds(duration);

        effectInstance.Stop();
        Destroy(effectInstance.gameObject);
    }

    private void StartImageAnimation()
    {
        if (abilityImage != null && !isAnimating)
        {
            LeanTween.scale(abilityImage.rectTransform, new Vector3(0.2f, 0.2f, 0.2f), 0.5f).setLoopPingPong();
            isAnimating = true;
        }
    }

    private void StopImageAnimation()
    {
        if (abilityImage != null && isAnimating)
        {
            LeanTween.cancel(abilityImage.rectTransform);
            abilityImage.rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            isAnimating = false;
        }
    }
}
