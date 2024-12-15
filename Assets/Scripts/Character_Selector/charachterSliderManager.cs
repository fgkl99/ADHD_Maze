using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSliderManager : MonoBehaviour
{
    public Sprite[] characterImages;             // Array of images for the characters
    public TextMeshProUGUI[] characterDescriptionsUI; // Array of TextMeshProUGUI fields for the descriptions
    public Image[] characterImageUI;             // Array of UI images to display characters
    public Slider characterSlider;               // Slider

    private void Start()
    {
        // Set the range for the slider
        characterSlider.minValue = 0;
        characterSlider.maxValue = characterImages.Length - 1;
        characterSlider.wholeNumbers = true;

        // Update the character display initially
        UpdateCharacterDisplay((int)characterSlider.value);

        // Add a listener to update the display when the slider changes
        characterSlider.onValueChanged.AddListener(delegate { UpdateCharacterDisplay((int)characterSlider.value); });
    }

    public void UpdateCharacterDisplay(int index)
    {
        // Check if the index is within range
        if (index < 0 || index >= characterImages.Length)
        {
            Debug.LogError($"Index out of bounds: {index}. Check the 'characterImages' array in the Inspector.");
            return;
        }

        // Hide all images
        foreach (var img in characterImageUI)
        {
            img.gameObject.SetActive(false);
        }

        // Show the selected character's image
        if (index < characterImageUI.Length)
        {
            characterImageUI[index].gameObject.SetActive(true);
        }

        // Hide all descriptions
        foreach (var description in characterDescriptionsUI)
        {
            description.gameObject.SetActive(false);
        }

        // Show the description of the selected character
        if (index < characterDescriptionsUI.Length)
        {
            characterDescriptionsUI[index].gameObject.SetActive(true);
        }
    }
    public void OnStart()
    {
        CrossVariables.character_selection = characterSlider.value;
        Debug.Log(CrossVariables.mood_of_today);
        Debug.Log(CrossVariables.character_selection);
        SceneManager.LoadScene("MazeGame");
    }
    public void OnBack()
    {
        SceneManager.LoadScene("MoodSelection");
    }
}
