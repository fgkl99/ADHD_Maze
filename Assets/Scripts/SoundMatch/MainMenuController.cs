using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    // Funzione per caricare la scena del gioco quando il pulsante è premuto
    public void StartGame()
    {
        // Carica la scena del gioco, assicurandosi che la scena "SampleScene" sia aggiunta nelle Build Settings
        SceneManager.LoadScene("SoundMemoryGame");
    }
}