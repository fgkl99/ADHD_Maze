using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SoundMemoryGame : MonoBehaviour
{
    public Button[] soundButtons;  // Assign the 4 buttons in the Inspector
    public Button startButton;
    public Button restartButton;  // New Restart Button
    public Button exitButton;     // New Exit Button
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI scoreText;
    public AudioClip[] sounds;    // Assign 4 sounds in the Inspector

    private List<int> sequence = new List<int>(); // Stores the sequence of sounds
    private int currentRound = 0;  // Keeps track of the current round
    private int playerIndex = 0;  // Tracks the player's progress in the sequence
    private int score = 0;

    private bool isGameActive = false; // Indicates whether the game is active
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Show the initial message to "Play the buttons"
        messageText.text = "Play the buttons";

        // Initially hide the score text and the restart/exit buttons
        scoreText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        // Attach Start Game function to Start Button
        startButton.onClick.AddListener(StartGame);

        // Attach listeners for sound buttons
        for (int i = 0; i < soundButtons.Length; i++)
        {
            int buttonIndex = i; // Capture index in a local variable to avoid closure issues
            soundButtons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));
        }

        // Attach listeners for Restart and Exit buttons
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        isGameActive = true; // Activate the game
        messageText.text = "Sound sequence begins...";
        startButton.gameObject.SetActive(false); // Hide the Start Button

        // Show the score text when the game starts
        scoreText.gameObject.SetActive(true);

        sequence.Clear(); // Clear any previous sequence
        currentRound = 0;
        playerIndex = 0;
        score = 0;
        scoreText.text = "Score: 0";
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(2f);

        // Add a new random sound to the sequence
        if (sequence.Count <= currentRound)
            sequence.Add(Random.Range(0, soundButtons.Length));

        // Play the sequence
        foreach (int index in sequence)
        {
            PlaySound(index);
            yield return new WaitForSeconds(1f); // Pause between sounds
        }

        messageText.text = "Repeat the sequence.";
        playerIndex = 0; // Reset player progress for this round
    }

    void OnButtonClick(int buttonIndex)
    {
        HighlightButton(buttonIndex); // Highlight the button only when clicked by the player
        PlaySound(buttonIndex); // Always play the sound for the clicked button

        if (!isGameActive) return; // If the game hasn’t started, do nothing else

        // Gameplay logic: Check if the player's input matches the sequence
        if (buttonIndex == sequence[playerIndex])
        {
            playerIndex++;

            // If the player completes the sequence
            if (playerIndex == sequence.Count)
            {
                currentRound++;
                score += 10;
                scoreText.text = "Score: " + score;

                if (currentRound == 6)
                {
                    messageText.text = "You win!";
                    isGameActive = false; // End the game
                    restartButton.gameObject.SetActive(true); // Show Restart Button
                    exitButton.gameObject.SetActive(true);    // Show Exit Button
                }
                else
                {
                    messageText.text = "Sound sequence begins...";
                    StartCoroutine(PlaySequence());
                }
            }
        }
        else
        {
            // Wrong input: Game Over
            messageText.text = "Game Over!";
            isGameActive = false; // Deactivate the game

            // Show Restart and Exit buttons when the player loses
            restartButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);

            Debug.Log("Game Over! Buttons visible: Restart and Exit");
        }
    }

    void PlaySound(int index)
    {
        audioSource.PlayOneShot(sounds[index]);
    }

    void HighlightButton(int index)
    {
        Image buttonImage = soundButtons[index].GetComponent<Image>();
        Color originalColor = buttonImage.color;
        buttonImage.color = Color.yellow; // Highlight color

        StartCoroutine(ResetButtonColor(buttonImage, originalColor));
    }

    IEnumerator ResetButtonColor(Image buttonImage, Color originalColor)
    {
        yield return new WaitForSeconds(0.5f);
        buttonImage.color = originalColor; // Restore original color
    }

    // Function to restart the game
    public void RestartGame()
    {
        Debug.Log("Restarting Game...");

        // Reset game state
        sequence.Clear();
        currentRound = 0;
        playerIndex = 0;
        score = 0;
        scoreText.text = "Score: 0";

        // Hide Restart and Exit buttons, show Start button again
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true); // Show Start Button again

        // Reset the message and score visibility
        messageText.text = "Play the buttons";
        scoreText.gameObject.SetActive(false);

        Debug.Log("Game Restarted.");
    }

    // Function to exit the game
    public void ExitGame()
    {

        CrossVariables.minigame_counter = CrossVariables.minigame_counter + 1;
        SceneManager.LoadScene("MazeGame");
    }
}
