using UnityEngine;
using UnityEngine.SceneManagement;

public class UIColorGameManager : MonoBehaviour
{
   
        public void OnBack(){
        CrossVariables.minigame_counter = CrossVariables.minigame_counter +1;
        SceneManager.LoadScene("MazeGame"); 

    }
}
