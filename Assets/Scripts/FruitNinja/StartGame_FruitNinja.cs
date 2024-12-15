using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame_FruitNinja : MonoBehaviour
{
    public void onStartGame()
    {
        SceneManager.LoadScene("FruitNinja");
    }
}
