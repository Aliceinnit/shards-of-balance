using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessManager : MonoBehaviour
{
    public string nextLevelSceneName;
    public void NextLevel()
    { 
        SceneManager.LoadScene(nextLevelSceneName);
    }
}
