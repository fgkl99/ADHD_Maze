using UnityEngine;

public class MusicManager_Meditation : MonoBehaviour
{
    public string[] musicTrackNames; // Names of audio files in the Resources folder
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        // Create an AudioSource dynamically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Do not loop; we'll control looping manually

        if (musicTrackNames.Length > 0)
        {
            PlayTrack(0); // Start with the first track
        }
    }

    public void PlayTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= musicTrackNames.Length)
        {
            Debug.LogWarning("Track index out of range.");
            return;
        }

        // Load the audio clip from Resources
        AudioClip clip = Resources.Load<AudioClip>(musicTrackNames[trackIndex]);
        if (clip == null)
        {
            Debug.LogWarning($"Audio file '{musicTrackNames[trackIndex]}' not found in Resources folder.");
            return;
        }

        audioSource.Stop(); // Stop the current track
        audioSource.clip = clip;
        audioSource.Play(); // Play the new track
        currentTrackIndex = trackIndex;
    }

    public void NextTrack()
    {
        int nextIndex = (currentTrackIndex + 1) % musicTrackNames.Length;
        PlayTrack(nextIndex);
    }

    public void PreviousTrack()
    {
        int previousIndex = (currentTrackIndex - 1 + musicTrackNames.Length) % musicTrackNames.Length;
        PlayTrack(previousIndex);
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause(); // Pause the music
        }
        else
        {
            audioSource.Play(); // Resume the music
        }
    }
}