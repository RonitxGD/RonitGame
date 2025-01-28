using TMPro;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public GameObject invulnerabilityModel;
    public GameObject speedBoostModel;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }


    public void OnEnemyDestroyed(GameObject enemy)
    {
        Vector3 deathPosition = enemy.transform.position;


        if (enemy.CompareTag("Boss"))
        {

            SpawnPickup(deathPosition);
        }
        else if (enemy.CompareTag("Enemy"))
        {

            float spawnChance = Random.Range(0f, 1f);
            if (spawnChance <= 0.35f)
            {
                SpawnPickup(deathPosition);
            }
        }
    }


    private void SpawnPickup(Vector3 spawnPosition)
    {

        GameObject pickupModel = Random.Range(0, 2) == 0 ? invulnerabilityModel : speedBoostModel;


        Instantiate(pickupModel, spawnPosition, Quaternion.identity);
    }
}
