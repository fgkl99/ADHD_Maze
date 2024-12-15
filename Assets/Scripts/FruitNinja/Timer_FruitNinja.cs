using UnityEngine;
using UnityEngine.UI;

public class Timer_FruitNinja : MonoBehaviour
{
    public float timeRemaining = 30f; // Tempo base di partenza
    public Text timerText; // Riferimento al testo del timer nella UI
    private float elapsedTime = 0f; // Variabile per registrare il tempo trascorso

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Riduce il tempo rimanente
            elapsedTime += Time.deltaTime;  // Calcola il tempo totale giocato
            UpdateTimerUI(); // Aggiorna il testo del timer
        }
        else
        {
            timeRemaining = 0; // Blocca il timer a 0
            UpdateTimerUI();
            FindObjectOfType<GameManager_FruitNinja>().Explode(); // Termina il gioco
        }
    }

    // Aggiunge tempo al timer
    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
        UpdateTimerUI(); // Aggiorna il testo ogni volta che si modifica il tempo
    }

    // Ottiene il tempo totale giocato
    public float GetElapsedTime()
    {
        return elapsedTime; // Ritorna il tempo trascorso dall'inizio
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Aggiorna il testo del timer
    }
}
