using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager_FruitNinja : MonoBehaviour
{
    // Riferimenti agli elementi dell'interfaccia e agli oggetti di gioco
    public Text scoreText; // Testo che mostra il punteggio attuale
    private Blade lama; // Riferimento alla lama (oggetto controllato dal giocatore)
    public Image fadeImage; // Immagine utilizzata per gli effetti di fade-in/fade-out
    private Spawner_FruitNinja spawner; // Riferimento al generatore di frutti
    private int score; // Variabile per memorizzare il punteggio attuale
    public Text finalScore; // Testo che mostrerà il punteggio finale
    public Text finalTime; // Testo che mostrerà il tempo impiegato
    public GameObject exitButton; // Riferimento al pulsante "Esci"

    private float startTime;


    private void Awake()
    {
        lama = FindAnyObjectByType<Blade>();
        spawner = FindAnyObjectByType<Spawner_FruitNinja>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lama.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();

        Time.timeScale = 1f;

        ClearScene();

        // Registra il tempo di inizio
        startTime = Time.time;

        // Nascondi i testi finali
        finalScore.gameObject.SetActive(false);
        finalTime.gameObject.SetActive(false);
        exitButton.SetActive(false); // Disattiva il tasto "Esci" all'inizio


    }


    private void ClearScene()
    {
        Fruit[] frutti = FindObjectsOfType<Fruit>();
        foreach (Fruit frutto in frutti)
        {
            Destroy(frutto.gameObject);
        }

        Bomba[] bombe = FindObjectsOfType<Bomba>();
        foreach (Bomba bomba in bombe)
        {
            Destroy(bomba.gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void LosePoint()
    {
        score = Mathf.Max(0, score - 1); // Riduci il punteggio di 1, ma non sotto 0
        scoreText.text = score.ToString(); // Aggiorna il punteggio
    }

    public void Explode()
    {
        lama.enabled = false; // Disabilita la lama
        spawner.enabled = false; // Ferma la generazione di frutti

        Time.timeScale = 0f; // Ferma il tempo di gioco

        // Mostra lo schermo bianco e i punteggi finali
        fadeImage.color = Color.white; // Rendi lo schermo bianco
        ShowFinalResults();
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);
        NewGame();

        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    private void ShowFinalResults()
    {
        // Mostra il punteggio finale e il tempo impiegato
        float timeElapsed = FindObjectOfType<Timer_FruitNinja>().GetElapsedTime();
        finalScore.text = $"Final Score: {score}";
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        finalTime.text = $"Time: {minutes:00}:{seconds:00}";
        // Mostra i risultati finali
        finalScore.gameObject.SetActive(true);
        finalTime.gameObject.SetActive(true);
        // Mostra il pulsante "Esci"
        exitButton.SetActive(true);

    }


    public void ExitGame()
    {
        // Esce dall'applicazione (funziona solo in build, non in editor)
        Application.Quit();
    }


}

