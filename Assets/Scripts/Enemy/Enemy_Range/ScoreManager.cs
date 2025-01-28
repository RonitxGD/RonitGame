using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public static ScoreManager instance;

    public int score = 0;
    public int scoreThreshold = 10; // Score required to boost player stats
    public float boostDuration = 10f; // Duration of the boost effect

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        if (score % scoreThreshold == 0)
        {
            //PlayerStats.instance.ApplyBoost(boostDuration);
        }
    }
}

