using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequenceGame : MonoBehaviour
{
    public Button[] buttons;
    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private bool isPlayerTurn = false;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI scoreText;

    private int score = 0;

    public GameObject exitButton;   // Exit button for the game
    public GameObject restartButton; // Restart button for the game

    public float initialTimeBetweenButtons = 0.3f; // Initial time between buttons
    public float timeDecreaseFactor = 0.3f; // Higher reduction in waiting time between buttons
    public float buttonHighlightTime = 0.15f; // Time a button remains highlighted

    void Start()
    {
        Debug.Log("Game start");
        UpdateScoreText();

        // Assicura che la musica sia gestita tramite MusicManager
        if (MusicManager.Instance != null && !MusicManager.Instance.IsMusicOn)
        {
            MusicManager.Instance.ToggleMusic(); // Ferma la musica se è spenta
        }

        if (messageText != null)
            messageText.text = "Game Start";

        // Nascondi i pulsanti Restart e Exit all'inizio del gioco
        if (exitButton != null)
            exitButton.SetActive(false);
        if (restartButton != null)
            restartButton.SetActive(false);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);

        messageText.text = "Starting new sequence";
        yield return new WaitForSeconds(0.8f);

        yield return Countdown(3); // Countdown from 3 to 1

        AddToSequence();
        yield return PlaySequence();

        isPlayerTurn = true;
        messageText.text = "Player's turn";
    }

    IEnumerator Countdown(int start)
    {
        for (int i = start; i > 0; i--)
        {
            messageText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    void AddToSequence()
    {
        int randomIndex = Random.Range(0, buttons.Length);
        sequence.Add(randomIndex);
        Debug.Log("New element added to the sequence: " + randomIndex);
    }

    IEnumerator PlaySequence()
    {
        isPlayerTurn = false;

        float timeBetweenButtons = initialTimeBetweenButtons;
        float timeDecreaseRate = timeDecreaseFactor * sequence.Count;

        foreach (int index in sequence)
        {
            yield return StartCoroutine(HighlightButton(buttons[index])); // Highlight button briefly

            // Assicura che il tempo tra i pulsanti sia maggiore di buttonHighlightTime
            timeBetweenButtons = Mathf.Max(timeBetweenButtons - timeDecreaseRate, buttonHighlightTime + 0.1f);

            yield return new WaitForSeconds(timeBetweenButtons); // Wait before the next button
        }
    }

    IEnumerator HighlightButton(Button button)
    {
        Vector3 originalScale = button.transform.localScale;
        Color originalColor = button.image.color;

        button.transform.localScale = originalScale * 1.1f;
        button.image.color = new Color(originalColor.r * 1.5f, originalColor.g * 1.5f, originalColor.b * 1.5f);

        yield return new WaitForSeconds(buttonHighlightTime);

        button.transform.localScale = originalScale;
        button.image.color = originalColor;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void OnButtonPress(int buttonIndex)
    {
        Debug.Log("Button pressed: " + buttonIndex);
        if (!isPlayerTurn) return;

        StartCoroutine(AnimateButtonPress(buttons[buttonIndex])); // Animate button press

        if (buttonIndex == sequence[playerIndex])
        {
            playerIndex++;
            Debug.Log("Correct button: " + buttonIndex);

            if (playerIndex >= sequence.Count)
            {
                playerIndex = 0;
                score += 10;
                UpdateScoreText();

                messageText.text = "Great!";
                StartCoroutine(WaitAndStartNextSequence(0.1f));
            }
        }
        else
        {
            Debug.Log("Game over: incorrect button pressed.");
            messageText.text = "Game Over.";
            scoreText.text = "Final score: " + score;
            StopGame();
        }
    }

    IEnumerator AnimateButtonPress(Button button)
    {
        Vector3 originalScale = button.transform.localScale;

        button.transform.localScale = originalScale * 1.1f; // Enlarge the button slightly
        yield return new WaitForSeconds(0.1f);
        button.transform.localScale = originalScale; // Return to original size
    }

    IEnumerator WaitAndStartNextSequence(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (score >= 100*CrossVariables.character_selection/4+30)
        {
            messageText.text = "You won! Final score: " + score;
            scoreText.text = "Final score: " + score;
            StopGame();
        }
        else
        {
            StartCoroutine(StartGame());
        }
    }

    void StopGame()
    {
        foreach (Button button in buttons)
            button.interactable = false;

        if (exitButton != null)
            exitButton.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting the game...");

        sequence.Clear();
        playerIndex = 0;
        score = 0;
        UpdateScoreText();

        if (exitButton != null)
            exitButton.SetActive(false);
        if (restartButton != null)
            restartButton.SetActive(false);

        foreach (Button button in buttons)
            button.interactable = true;

        StartCoroutine(StartGame());
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        CrossVariables.minigame_counter = CrossVariables.minigame_counter +1;
        SceneManager.LoadScene("MazeGame"); 
    }
}
