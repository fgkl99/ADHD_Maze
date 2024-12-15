using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject musicButton;  // Riferimento al pulsante per la musica
    private Text buttonText; // Riferimento al testo del pulsante

    void Start()
    {
        // Assicurati che il pulsante abbia un testo da aggiornare
        if (musicButton != null)
        {
            buttonText = musicButton.GetComponentInChildren<Text>();
        }

        // Imposta il testo iniziale del pulsante
        if (buttonText != null)
        {
            buttonText.text = MusicManager.Instance != null && MusicManager.Instance.IsMusicOn ? "Music On" : "Music Off";
        }
    }

    // Metodo per il pulsante "Music on/off"
    public void ToggleMusic()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.ToggleMusic(); // Alterna lo stato della musica
            // Aggiorna il testo del pulsante
            if (buttonText != null)
            {
                buttonText.text = MusicManager.Instance.IsMusicOn ? "Music On" : "Music Off";
            }
        }
    }

    // Metodo per il pulsante "Start"
    public void StartGame()
    {
        // Salva la scelta della musica prima di cambiare scena
        PlayerPrefs.SetInt("MusicOn", MusicManager.Instance.IsMusicOn ? 1 : 0);
        SceneManager.LoadScene("RicordaLaSequenza"); // Carica la scena "RicordaLaSequenza"
    }
}