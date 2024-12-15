using UnityEngine;
using UnityEngine.SceneManagement;



public class UiManager_CercaOggetto : MonoBehaviour
{

   
       public void OnBack()
{
    Debug.Log("OnBack cliccato! Minigame counter: " + CrossVariables.minigame_counter);
    CrossVariables.minigame_counter += 1;
    SceneManager.LoadScene("MazeGame");
}

}


