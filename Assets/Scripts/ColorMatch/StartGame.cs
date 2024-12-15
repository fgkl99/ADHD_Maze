using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void onStartGame()
    {
        SceneManager.LoadScene("ColorMatch");
    }
}
