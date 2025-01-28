using System.Collections;
using UnityEngine;
using TMPro; 

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 10;
    public float timeBetweenWaves = 5f;
    public float spawnInterval = 0.5f;
    public float spawnIntervalDecreaseRate = 0.05f;

    [Header("Boss Settings")]
    public int bossWaveInterval = 5;
    public GameObject[] bossPrefabs;
    public Transform bossSpawnPoint;

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    [Header("UI Settings")]
    public TMP_Text waveText;
    public GameObject wavePopup; // UI pop-up for wave announcement
    public TMP_Text wavePopupText; // Text inside the pop-up

    [Header("Audio Settings")]
    public AudioClip waveStartClip; // Audio to play at the start of each wave
    public AudioSource audioSource; // Audio source to play the clip

    private int currentWave = 0;
    private bool spawningWave = false;

    void Start()
    {
        if (waveText == null)
        {
            Debug.LogError("Wave Text UI is not assigned!");
            return;
        }

        if (wavePopup == null)
        {
            Debug.LogError("Wave Popup UI is not assigned!");
            return;
        }

        wavePopup.SetActive(false); // Ensure the pop-up is hidden initially
        StartCoroutine(WaveSpawner());
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
            spawningWave = true;

            UpdateWaveUI();

            // Play the wave start audio
            if (audioSource != null && waveStartClip != null)
            {
                audioSource.PlayOneShot(waveStartClip);
            }

            // Show the wave pop-up
            StartCoroutine(ShowWavePopup($"Wave {currentWave} Begins!"));

            spawnInterval = Mathf.Max(0.2f, spawnInterval - spawnIntervalDecreaseRate);

            if (currentWave % bossWaveInterval == 0)
            {
                SpawnBoss();
            }
            else
            {
                yield return StartCoroutine(SpawnEnemies());
            }

            spawningWave = false;
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBoss()
    {
        if (bossPrefabs.Length > 0 && bossSpawnPoint != null)
        {
            GameObject bossPrefab = ChooseBoss();
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log($"Boss spawned on wave {currentWave}!");
        }
        else
        {
            Debug.LogWarning("BossPrefabs or BossSpawnPoint is not assigned!");
        }
    }

    GameObject ChooseBoss()
    {
        if (currentWave % (bossWaveInterval * 2) == 0)
        {
            return bossPrefabs[1];
        }
        else
        {
            return bossPrefabs[0];
        }
    }

    void UpdateWaveUI()
    {
        waveText.text = "Wave " + currentWave;
    }

    IEnumerator ShowWavePopup(string message)
    {
        wavePopupText.text = message;
        wavePopup.SetActive(true);
        yield return new WaitForSeconds(2f); // Show the pop-up for 2 seconds
        wavePopup.SetActive(false);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public bool IsSpawningWave()
    {
        return spawningWave;
    }
}
