using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 25f + CrossVariables.character_selection*2f; // Tempo iniziale in secondi
    public bool isTimerRunning = true; // Flag che indica se il timer è attivo
    public Text timerText; // Riferimento al testo UI per il timer
    public static bool isGameOver = false; // Flag per sapere se il gioco è finito

    private float noInputTime = 0f; // Tempo trascorso senza input
    private const float maxNoInputTime = 5f; // Tempo massimo consentito senza input

    void Update()
    {
        if (isTimerRunning)
        {
            // Riduce il tempo rimanente ogni frame
            timeRemaining -= Time.deltaTime;
            noInputTime += Time.deltaTime; // Incrementa il tempo senza input

            // Controlla se il tempo è scaduto
            if (timeRemaining <= 0)
            {
                timeRemaining = 0; // Evita che il tempo scenda sotto zero
                isTimerRunning = false; // Ferma il timer
                OnTimerEnd(); // Gestisce la fine del gioco
            }

            // Aggiorna il testo del timer
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (isTimerRunning)
        {
            // Mostra il tempo rimanente arrotondato
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
        }
    }

    public void AddTime(float seconds)
    {
        // Aggiunge secondi al tempo rimanente
        timeRemaining += seconds;
    }

    void OnTimerEnd()
    {
        // Mostra un messaggio di fine gioco
        Debug.Log("Timer finito!");
        timerText.text = "Tempo finito";
        isGameOver = true; // Imposta lo stato del gioco come terminato
    }

}
