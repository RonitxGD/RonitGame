using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HigscoreMng : MonoBehaviour
{
    public int highScore = 0;
    public int currentScore = 0; // Track the current score

    public TextMeshProUGUI highScoreText; // Display the high score
    public TextMeshProUGUI currentScoreText; // Display the current score (optional)

    private float scoreTimer = 0f; // Timer to control the score increment rate
    public float scoreIncrementRate = 1f; // Time in seconds between score increments

    void Start()
    {
        // Set the current score and high score text at the start of the game
        UpdateScoreUI();
    }

    void Update()
    {
        // Increment the score over time (adjust scoreIncrementRate as needed)
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= scoreIncrementRate)
        {
            // Increment the current score by 1
            currentScore += 1;

            // Update the high score if the current score is higher
            UpdateHighScore(currentScore);

            // Reset the timer
            scoreTimer = 0f;

            // Update the score UI
            UpdateScoreUI();
        }
    }

    // Returns the high score
    public int GetHighScore()
    {
        return highScore;
    }

    // Update the high score if the current score is higher
    public void UpdateHighScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;

            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + highScore.ToString();
            }
        }
    }

    // Update both current and high score UI elements
    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + currentScore.ToString();
        }
    }
}
