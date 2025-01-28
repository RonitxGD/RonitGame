using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameUIPanel;
    private bool isGamePaused = false;

    private void Start()
    {
        if (gameUIPanel != null)
        {
            gameUIPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameUI();
        }
    }

    private void ToggleGameUI()
    {
        if (gameUIPanel != null)
        {
            bool isActive = !gameUIPanel.activeSelf;
            gameUIPanel.SetActive(isActive);
            isGamePaused = isActive;

            Time.timeScale = isGamePaused ? 0 : 1;
        }
    }

    // Method to restart the current scene
    public void RestartScene()
    {
        // Unpause the game
        Time.timeScale = 1;

        // Get the name of the current scene
        string currentScene = SceneManager.GetActiveScene().name;

        // Load the current scene again, effectively restarting it
        SceneManager.LoadScene(currentScene);
    }
}
