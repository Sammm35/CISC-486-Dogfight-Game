using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
