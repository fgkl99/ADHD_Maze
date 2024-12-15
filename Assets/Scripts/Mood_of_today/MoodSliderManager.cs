using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodSliderManager : MonoBehaviour
{
    public Slider moodSlider;               // Slider per controllare la dimensione
    public RectTransform innerCircle;      // Cerchio interno
    public RectTransform outerCircle;      // Cerchio esterno
    public SpriteRenderer innerCircleRenderer; // SpriteRenderer di innerCircle
    public SpriteRenderer outerCircleRenderer; // SpriteRenderer di outerCircle
                                               // public Image sliderFillImage;          // Immagine del Fill Area dello slider

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;    // Componente AudioSource per la musica
    public AudioClip[] unpleasantClips;    // Array di tracce per -2
    public AudioClip[] slightlyUnpleasantClips; // Array di tracce per -1
    public AudioClip[] neutralClips;       // Array di tracce per 0
    public AudioClip[] slightlyPleasantClips; // Array di tracce per 1
    public AudioClip[] pleasantClips;      // Array di tracce per 2

    private float savedSliderValue;        // Valore salvato dello slider

    [Header("UI Elements")]
    public
    TextMeshProUGUI topText;   // Static text above the circles (e.g., "Mood of Today")
    public TextMeshProUGUI stateText; // Dynamic text below the slider (e.g., "Very Pleasant", "Neutral")

    [Header("Dimension Settings")]
    public float minScaleRatio = 0.1f;     // InnerCircle minimo (10% di outerCircle)
    public float maxScaleRatio = 1f;       // InnerCircle massimo (100% di outerCircle)

    // Colori per lo slider e i cerchi
    public Color negativeColor = new Color(0.0f, 0.0f, 0.5f);  // Blu Navy
    public Color positiveColor = new Color(0.498f, 1.0f, 0.831f); // Verde Acqua Chiaro
    public float outerPastelFactor = 0.6f; // Fattore pastello per outerCircle (effetto di saturazione)

    // Parametri della pulsazione
    [Header("Pulse Settings")]
    public float pulseIntensity = 0.02f;   // Intensità della pulsazione (2%)
    public float pulseSpeed = 1.5f;        // Velocità della pulsazione (1.5 pulsazioni al secondo)

    private float maxOuterScale;
    private float baseInnerScale;         // Scala di base di innerCircle
    private bool isDragging = false;      // Stato dello slider (in movimento o fermo)

    void Start()
    {
        // Imposta il range dello slider
        moodSlider.minValue = -2;         // Valore minimo
        moodSlider.maxValue = 2;          // Valore massimo
        moodSlider.value = 0;             // Posizione neutra iniziale

        // Abilita il movimento per numeri interi
        moodSlider.wholeNumbers = true;

        // Calcola la dimensione massima di outerCircle
        maxOuterScale = outerCircle.localScale.x;

        // Aggiorna il testo superiore
        topText.text = "How you doing today?";

        // Recupera il nome della traccia salvata
        string savedMusicName = PlayerPrefs.GetString("CurrentMusic", "");
        if (!string.IsNullOrEmpty(savedMusicName))
        {
            // Trova la traccia salvata e riproducila
            AudioClip savedClip = FindAudioClipByName(savedMusicName);
            if (savedClip != null)
            {
                backgroundMusic.clip = savedClip;
                backgroundMusic.Play();
            }
        }

        // Aggiorna inizialmente il testo dello stato e i cerchi
        UpdateStateText(moodSlider.value);
        UpdateCircle(moodSlider.value);
        // UpdateSliderFill(moodSlider.value);

        // Aggiungi un listener per aggiornare dinamicamente lo stato, il cerchio e lo slider
        moodSlider.onValueChanged.AddListener(UpdateCircle);
        //moodSlider.onValueChanged.AddListener(UpdateSliderFill);
        moodSlider.onValueChanged.AddListener(UpdateStateText);
        moodSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Carica il valore salvato dello slider
        savedSliderValue = PlayerPrefs.GetFloat("SavedSliderValue", 0); // Default a 0
        moodSlider.value = savedSliderValue;

        // Imposta la musica iniziale
        UpdateBackgroundMusic(savedSliderValue);

        // Listener per aggiornare la musica quando lo slider cambia
        moodSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        // Applica la pulsazione solo quando lo slider è fermo
        if (!isDragging)
        {
            ApplyPulse();
        }
    }

    void UpdateCircle(float value)
    {
        // Mappa il valore dello slider (-2 a 2) in un range [0, 1]
        float t = (value + 2) / 4f;

        // Calcola la dimensione di innerCircle
        baseInnerScale = Mathf.Lerp(minScaleRatio * maxOuterScale, maxScaleRatio * maxOuterScale, t);
        innerCircle.localScale = Vector3.one * baseInnerScale;

        // Cambia i colori dei cerchi per sincronizzarli con lo slider
        UpdateCircleColors(t);
    }

    // void UpdateSliderFill(float value)
    //{
    // Mappa da [-2, 2] a [0, 1]
    //  float t = (value + 2) / 4f;
    //sliderFillImage.color = Color.Lerp(negativeColor, positiveColor, t);
    //}

    private void UpdateCircleColors(float t)
    {
        // Cambia il colore di innerCircle in base al valore dello slider
        Color innerColor = Color.Lerp(negativeColor, positiveColor, t);
        innerCircleRenderer.color = innerColor;

        // Cambia il colore di outerCircle (aggiungendo un effetto pastello)
        Color outerColor = AdjustToPastel(innerColor, outerPastelFactor);
        outerCircleRenderer.color = outerColor;
    }

    private Color AdjustToPastel(Color baseColor, float factor)
    {
        // Rende il colore più pastello (meno saturo)
        return Color.Lerp(baseColor, Color.white, factor);
    }

    private void ApplyPulse()
    {
        if (innerCircle != null)
        {
            // Calcola la pulsazione (sinusoidale)
            float pulseFactor = 1f + pulseIntensity * Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2);

            // Limita la pulsazione a non superare la dimensione di outerCircle
            float pulseScale = Mathf.Min(baseInnerScale * pulseFactor, maxOuterScale);

            // Applica la nuova scala a innerCircle
            innerCircle.localScale = Vector3.one * pulseScale;
        }
    }

    public void UpdateStateText(float value)
    {
        // Cambia il testo dello stato in base al valore dello slider
        switch (value)
        {
            case -2:
                stateText.text = "Unpleasant";
                break;
            case -1:
                stateText.text = "Slightly Unpleasant";
                break;
            case 0:
                stateText.text = "Neutral";
                break;
            case 1:
                stateText.text = "Slightly Pleasant";
                break;
            case 2:
                stateText.text = "Pleasant";
                break;
        }
    }

    void OnSliderValueChanged(float value)
    {
        // Salva il valore dello slider
        PlayerPrefs.SetFloat("SavedSliderValue", value);
        PlayerPrefs.Save();

        // Cambia la musica in base al valore dello slider
        UpdateBackgroundMusic(value);
    }

    void UpdateBackgroundMusic(float value)
    {
        if (backgroundMusic == null) return;

        AudioClip selectedClip = null;

        // Seleziona casualmente una traccia dall'array corrispondente al valore dello slider
        if (value == -2 && unpleasantClips.Length > 0)
            selectedClip = unpleasantClips[Random.Range(0, unpleasantClips.Length)];
        else if (value == -1 && slightlyUnpleasantClips.Length > 0)
            selectedClip = slightlyUnpleasantClips[Random.Range(0, slightlyUnpleasantClips.Length)];
        else if (value == 0 && neutralClips.Length > 0)
            selectedClip = neutralClips[Random.Range(0, neutralClips.Length)];
        else if (value == 1 && slightlyPleasantClips.Length > 0)
            selectedClip = slightlyPleasantClips[Random.Range(0, slightlyPleasantClips.Length)];
        else if (value == 2 && pleasantClips.Length > 0)
            selectedClip = pleasantClips[Random.Range(0, pleasantClips.Length)];

        if (selectedClip != null)
        {
            // Salva il nome della traccia selezionata in PlayerPrefs
            PlayerPrefs.SetString("CurrentMusic", selectedClip.name);
            PlayerPrefs.Save();

            // Cambia e riproduci la traccia
            if (backgroundMusic.clip != selectedClip)
            {
                backgroundMusic.clip = selectedClip;
                backgroundMusic.Play();
            }
        }
    }
    private AudioClip FindAudioClipByName(string name)
    {
        foreach (AudioClip clip in unpleasantClips)
            if (clip.name == name) return clip;
        foreach (AudioClip clip in slightlyUnpleasantClips)
            if (clip.name == name) return clip;
        foreach (AudioClip clip in neutralClips)
            if (clip.name == name) return clip;
        foreach (AudioClip clip in slightlyPleasantClips)
            if (clip.name == name) return clip;
        foreach (AudioClip clip in pleasantClips)
            if (clip.name == name) return clip;

        return null;
    }



    public void OnSliderDragStart()
    {
        isDragging = true; // Disabilita la pulsazione durante il drag
    }

    public void OnSliderDragEnd()
    {
        isDragging = false; // Riabilita la pulsazione quando lo slider è fermo
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Quando si modificano i valori nell'Inspector, aggiorna i colori
        if (moodSlider != null)
        {
            float t = (moodSlider.value + 2) / 4f;
            UpdateCircleColors(t);
        }
    }
#endif
    private void OnDestroy()
    {
        // Removes all listeners to prevent memory leaks when the script is destroyed.
        moodSlider.onValueChanged.RemoveListener(UpdateCircle);
        moodSlider.onValueChanged.RemoveListener(UpdateStateText);
        moodSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public void OnNext()
    {

        CrossVariables.mood_of_today = moodSlider.value;
        Debug.Log(CrossVariables.mood_of_today);
        SceneManager.LoadScene("CharacterSelection");
    }


}

