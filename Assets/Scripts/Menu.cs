using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneTag.Battle.ToString());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
