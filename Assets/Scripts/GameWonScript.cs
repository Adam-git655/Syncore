using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWonScript : MonoBehaviour
{
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
