using UnityEngine;
using UnityEngine.UI;  
using TMPro;  

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3000;
    private int currentHealth;

    public bool isInvulnerable = false;

    [Header("UI Elements")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public GameObject loseUI; // Reference to the Lose UI (Try Again button)

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health slider and text
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        }

        if (loseUI != null)
        {
            loseUI.SetActive(false); // Ensure it's hidden at the start
        }

        // Ensure the game is unpaused at the beginning
        Time.timeScale = 1;
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;

            // Update UI
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Update UI
        UpdateHealthUI();
    }

    public void SetInvulnerable(bool status)
    {
        isInvulnerable = status;

        if (status)
        {
            // Start a coroutine to remove invulnerability after 10 seconds
            StartCoroutine(RemoveInvulnerabilityAfterDelay(10f));
        }
    }

    private System.Collections.IEnumerator RemoveInvulnerabilityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInvulnerable = false;
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        // Pause the game
        Time.timeScale = 0;

        // Show the "Try Again" button (Lose UI)
        if (loseUI != null)
        {
            loseUI.SetActive(true);
        }
    }
}
