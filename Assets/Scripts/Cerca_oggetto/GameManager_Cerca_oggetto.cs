using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager_Cerca_oggetto : MonoBehaviour
{
    public static GameManager_Cerca_oggetto Instance; // Riferimento al GameManager per accesso globale

    public string targetFruit; // Nome del frutto che il giocatore deve trovare
    public int score = 0; // Punteggio del giocatore
    public float timeRemaining = 30f; // Tempo rimanente per completare il gioco

    public Text scoreText; // Riferimento al componente UI del punteggio
    public Text timerText; // Riferimento al componente UI del timer
    public Text targetFruitText; // Riferimento al componente UI per mostrare il frutto da cercare
    public TextMeshProUGUI resultText;

    private int correctFruitsRemaining; // Numero di frutti giusti ancora da trovare
    public Button messageBackground; // Riferimento allo sfondo della scritta finale

    private void Awake()
    {
        // Assicura che ci sia solo una istanza del GameManager
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        messageBackground.gameObject.SetActive(false); // Non mostrare il bottone
        SelectRandomTargetFruit(); // Seleziona un frutto casuale da trovare
        CountCorrectFruits(); // Conta i frutti giusti (uguali al target)
        UpdateUI(); // Aggiorna la UI all'inizio del gioco

    }

    private void Update()
    {
        // Aggiorna il timer durante il gioco
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI(); // Aggiorna l'interfaccia utente
        }
        else
        {
            timeRemaining = 0;
            EndGame(false); // Tempo scaduto, termina il gioco con sconfitta
        }
    }

    public bool IsCorrectFruit(string fruitName)
    {
        return fruitName == targetFruit; // Controlla se il nome del frutto è uguale al target
    }

    public void OnCorrectFruitSelected(GameObject fruit)
    {
        Destroy(fruit); // Distruggi il frutto selezionato
        score++; // Incrementa il punteggio
        correctFruitsRemaining--; // Decrementa il numero di frutti giusti ancora da trovare
        UpdateUI(); // Aggiorna la UI

        // Se tutti i frutti giusti sono stati trovati, termina il gioco con vittoria
        if (correctFruitsRemaining == 0)
        {
            EndGame(true); // Fine del gioco con successo
        }
    }

    public void OnWrongFruitSelected()
    {
        timeRemaining += 1; // Aggiungi 1 secondo per ogni frutto sbagliato selezionato
        UpdateUI(); // Aggiorna la UI
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString(); // Aggiorna il punteggio
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString(); // Aggiorna il timer
        targetFruitText.text =  targetFruit; // Aggiorna il nome del frutto da cercare
    }

   private void EndGame(bool success)
{
    if (success)
    {
        resultText.text = "You won!, go back!";
    }
    else
    {
        resultText.text = "You lost! Retry!";
    }

    
    messageBackground.gameObject.SetActive(true); // Mostra lo sfondo
    Time.timeScale = 0; // Ferma il gioco
}

    private void SelectRandomTargetFruit()
    {
        // Trova tutti i frutti nella scena
        FruitBehavior[] fruits = FindObjectsOfType<FruitBehavior>();

        if (fruits.Length > 0)
        {
            // Seleziona un frutto casuale e imposta il suo nome come target
            targetFruit = fruits[Random.Range(0, fruits.Length)].fruitName;
        }
        else
        {
            Debug.LogWarning("Nessun frutto trovato nella scena!"); // Se non ci sono frutti nella scena, avvisa
        }
    }

    private void CountCorrectFruits()
    {
        // Conta il numero di frutti che corrispondono al target nella scena
        correctFruitsRemaining = 0;
        FruitBehavior[] fruits = FindObjectsOfType<FruitBehavior>();

        foreach (FruitBehavior fruit in fruits)
        {
            if (fruit.fruitName == targetFruit)
            {
                correctFruitsRemaining++; // Incrementa se il frutto è quello giusto
            }
        }

        Debug.Log("Frutti corretti iniziali: " + correctFruitsRemaining); // Log per il debug
    }




}
