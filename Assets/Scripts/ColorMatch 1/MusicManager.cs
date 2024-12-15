using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; // Riferimento al singleton
    public AudioClip musicClip; // Clip musicale
    private AudioSource audioSource; // Audio source per la musica
    public bool IsMusicOn { get; private set; } = true; // Stato della musica

    void Awake()
    {
        // Verifica se c'è già un'istanza di MusicManager, se sì, distruggi questo oggetto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantieni MusicManager tra le scene
        }

        // Aggiungi l'AudioSource se non presente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Carica la preferenza salvata (musica on/off)
        IsMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1; // 1 per attivo, 0 per disattivo

        // Imposta la clip musicale e avvia la musica se è attivata
        audioSource.clip = musicClip;
        audioSource.loop = true;

        if (IsMusicOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    // Metodo per attivare o disattivare la musica
    public void ToggleMusic()
    {
        if (IsMusicOn)
        {
            audioSource.Stop(); // Ferma la musica
        }
        else
        {
            audioSource.Play(); // Avvia la musica
        }
        IsMusicOn = !IsMusicOn; // Inverti lo stato della musica

        // Salva la preferenza
        PlayerPrefs.SetInt("MusicOn", IsMusicOn ? 1 : 0);
    }
}