using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string nextLevelSceneName;
    public void PlayGame()
    {
        Debug.Log("Button clicked");
        Debug.Log("Trying to load scene: " + nextLevelSceneName);
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void QuitGame()
{
    Debug.Log("Quit Game");

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
