using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private bool isInvulnerable = false;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText; // Display health in text format

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize health UI
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;

            // Update health text
            if (healthText != null)
            {
                healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
            }

            // Check if the enemy is dead
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void SetInvulnerable(bool status)
    {
        isInvulnerable = status;
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
