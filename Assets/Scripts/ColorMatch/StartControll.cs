using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControll : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ColorMatch"); 
    }
}
