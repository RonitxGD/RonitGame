using UnityEngine;

public class BulletScore : MonoBehaviour
{
    [SerializeField] private int scoreForEnemy = 10; // Score for hitting an enemy
    [SerializeField] private int scoreForBoss = 50; // Score for hitting a boss

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy
        if (other.CompareTag("Enemy"))
        {
            IncreaseScore(scoreForEnemy);
            Debug.Log("Bullet hit an enemy! Score increased by " + scoreForEnemy);

            // Destroy the enemy (optional)
            Destroy(other.gameObject);
        }

        // Check if the bullet hits a boss
        else if (other.CompareTag("Boss"))
        {
            IncreaseScore(scoreForBoss);
            Debug.Log("Bullet hit a boss! Score increased by " + scoreForBoss);

            // Destroy the boss (optional)
            Destroy(other.gameObject);
        }

        // Destroy the bullet after hitting
        Destroy(gameObject);
    }

    private void IncreaseScore(int amount)
    {
        // Access ScoreMng and increase the score
        ScoreMng scoreMng = FindObjectOfType<ScoreMng>();
        if (scoreMng != null)
        {
            scoreMng.IncreaseScore(amount);
        }
        else
        {
            Debug.LogError("ScoreMng not found in the scene!");
        }
    }
}
