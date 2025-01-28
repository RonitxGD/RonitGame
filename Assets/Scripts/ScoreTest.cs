using UnityEngine;

public class ScoreTest : MonoBehaviour
{
    [SerializeField] private ScoreMng scoreMng; // Reference to your ScoreMng script
    [SerializeField] private float scoreIncreaseInterval = 1f; // Time interval for score increase
    [SerializeField] private int scoreIncrement = 10; // How much to increase the score each time

    private float timer;

    void Start()
    {
        if (scoreMng == null)
        {
            scoreMng = FindObjectOfType<ScoreMng>();
            if (scoreMng == null)
            {
                Debug.LogError("ScoreMng not found in the scene.");
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= scoreIncreaseInterval)
        {
            timer = 0f;
            IncreaseScore();
        }
    }

    private void IncreaseScore()
    {
        if (scoreMng != null)
        {
            scoreMng.IncreaseScore(scoreIncrement);
            //Debug.Log($"Score increased by {scoreIncrement}. Current score: {scoreMng.GetCurrentScore()}");
        }
    }
}
