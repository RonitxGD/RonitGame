using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PickupManager pickupManager;

    [SerializeField] private int scoreValue = 10;

    public void OnEnemyDestroyed()
    {
      
        Debug.Log($"{gameObject.name} has been destroyed!");

        if (pickupManager != null)
        {
         
            pickupManager.OnEnemyDestroyed(gameObject);
        }
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Increase score when hit by a bullet
            ScoreMng scoreMng = FindObjectOfType<ScoreMng>();
            if (scoreMng != null)
            {
                scoreMng.IncreaseScore(scoreValue);
            }

            // Destroy enemy and bullet
            Destroy(other.gameObject); // Destroy bullet
            Destroy(gameObject); // Destroy enemy
        }
    }
}
