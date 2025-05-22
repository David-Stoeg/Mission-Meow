using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMainMenu : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
