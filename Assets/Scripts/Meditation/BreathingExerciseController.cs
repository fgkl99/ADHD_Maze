using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BreathingExerciseController : MonoBehaviour
{
    // UI elements
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI timerText;
    public Slider breathingSlider;
    public Image smallBalloon;
    public Image largeBalloon;

    public Button restartButton; // Restart button
    public Button exitButton;    // Exit button

    // Game parameters
    private float inhaleDuration = 4f;
    private float holdDuration = 4f;
    private float exhaleDuration = 4f;
    private float timer = 0f;
    private int phase = -1; // -1: Waiting to start, 0: Inhale, 1: Hold (Inhale), 2: Exhale, 3: Hold (Exhale)

    private float sliderStartValue = 0f;
    private float sliderEndValue = 1f;

    // Countdown parameters
    private float countdownTime = 1f; // Time per countdown number (3, 2, 1)
    private int countdownNumber = 3; // Start countdown at 3

    // Delay parameters
    private float welcomeDelayTime = 3f; // Time to wait before showing second message
    private float secondMessageDelayTime = 3f; // Time to wait before starting countdown after second message
    private bool hasDisplayedWelcome = false;
    private bool hasDisplayedSecondMessage = false;

    // Cycle parameters
    private int totalCycles = 2; // Total number of cycles
    private int completedCycles = 0;
    private bool isExerciseComplete = false;

    // End messages
    private bool hasDisplayedFirstEndMessage = false;
    private bool hasDisplayedSecondEndMessage = false;

    private void Start()
    {
        // Initialize game
        breathingSlider.value = sliderStartValue;
        instructionText.text = "Welcome to this new practice";
        timerText.text = "";

        // Initially hide the buttons
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        // Attach listeners for the buttons
        restartButton.onClick.AddListener(RestartExercise);
        exitButton.onClick.AddListener(ExitExercise);
    }

    private void Update()
    {
        if (isExerciseComplete)
        {
            HandleEndMessages();
            return;
        }

        // Existing breathing logic
        if (!hasDisplayedWelcome)
        {
            timer += Time.deltaTime;
            if (timer >= welcomeDelayTime)
            {
                hasDisplayedWelcome = true;
                timer = 0f; // Reset the timer for the second message
                instructionText.text = "Follow the messages, relax"; // Show the second message
            }
        }
        else if (!hasDisplayedSecondMessage)
        {
            timer += Time.deltaTime;
            if (timer >= secondMessageDelayTime)
            {
                hasDisplayedSecondMessage = true;
                timer = 0f; // Reset the timer for the countdown
                instructionText.text = countdownNumber.ToString(); // Show the countdown number
            }
        }
        else if (phase == -1) // Countdown phase
        {
            timer += Time.deltaTime;
            if (timer >= countdownTime)
            {
                timer = 0f;
                countdownNumber--;
                if (countdownNumber > 0)
                {
                    instructionText.text = countdownNumber.ToString(); // Show countdown number
                }
                else
                {
                    phase = 0; // Start the breathing exercise
                    instructionText.text = "Inhale";
                    timerText.text = "4"; // Start the inhale phase timer
                }
            }
        }
        else
        {
            HandleBreathingPhases();
        }
    }

    private void HandleBreathingPhases()
    {
        timer += Time.deltaTime;

        switch (phase)
        {
            case 0: // Inhale phase
                if (timer <= inhaleDuration)
                {
                    timerText.text = Mathf.Ceil(inhaleDuration - timer).ToString();
                    breathingSlider.value = Mathf.Lerp(sliderStartValue, sliderEndValue, timer / inhaleDuration);
                    smallBalloon.rectTransform.localScale = Vector3.Lerp(Vector3.one * 0.5f, largeBalloon.rectTransform.localScale, timer / inhaleDuration);
                }
                else
                {
                    phase = 1;
                    timer = 0f;
                    instructionText.text = "Hold";
                    breathingSlider.value = sliderEndValue;
                }
                break;

            case 1: // Hold Breath (Inhale) phase
                if (timer <= holdDuration)
                {
                    timerText.text = Mathf.Ceil(holdDuration - timer).ToString();
                }
                else
                {
                    phase = 2;
                    timer = 0f;
                    instructionText.text = "Exhale";
                }
                break;

            case 2: // Exhale phase
                if (timer <= exhaleDuration)
                {
                    timerText.text = Mathf.Ceil(exhaleDuration - timer).ToString();
                    breathingSlider.value = Mathf.Lerp(sliderEndValue, sliderStartValue, timer / exhaleDuration);
                    smallBalloon.rectTransform.localScale = Vector3.Lerp(largeBalloon.rectTransform.localScale, Vector3.one * 0.5f, timer / exhaleDuration);
                }
                else
                {
                    phase = 3;
                    timer = 0f;
                    instructionText.text = "Hold";
                }
                break;

            case 3: // Hold Breath (Exhale) phase
                if (timer <= holdDuration)
                {
                    timerText.text = Mathf.Ceil(holdDuration - timer).ToString();
                }
                else
                {
                    completedCycles++;
                    if (completedCycles < totalCycles)
                    {
                        phase = 0;
                        timer = 0f;
                        instructionText.text = "Inhale";
                    }
                    else
                    {
                        isExerciseComplete = true;
                        instructionText.text = "";
                        timerText.text = "";
                        ShowEndButtons();
                    }
                }
                break;
        }
    }

    private void HandleEndMessages()
    {
        timer += Time.deltaTime;

        if (!hasDisplayedFirstEndMessage && timer >= 2f)
        {
            instructionText.text = "Daily practice completed, good job!";
            timer = 0f;
            hasDisplayedFirstEndMessage = true;
        }
        else if (hasDisplayedFirstEndMessage && !hasDisplayedSecondEndMessage && timer >= 3f)
        {
            instructionText.text = "See you tomorrow, take care :)";
            hasDisplayedSecondEndMessage = true;
        }
    }

    private void ShowEndButtons()
    {
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    public void RestartExercise()
    {
        // Restart the exercise logic
        phase = -1;
        timer = 0f;
        completedCycles = 0;
        hasDisplayedWelcome = false;
        hasDisplayedSecondMessage = false;
        hasDisplayedFirstEndMessage = false;
        hasDisplayedSecondEndMessage = false;
        isExerciseComplete = false;

        instructionText.text = "Welcome to this new practice";
        timerText.text = "";
        breathingSlider.value = sliderStartValue;

        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void ExitExercise()
    {
        CrossVariables.meditation_done = true;
        SceneManager.LoadScene("MazeGame");
    }
}
