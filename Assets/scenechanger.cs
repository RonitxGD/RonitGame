using UnityEngine;
using UnityEngine.SceneManagement;
public class scenechanger : MonoBehaviour
{
    // Call this method to load a scene by name
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Call this method to reload the current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Optional: Call this method to quit the game (works in a built version, not in the editor)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); // This log will show in the editor
    }
}
