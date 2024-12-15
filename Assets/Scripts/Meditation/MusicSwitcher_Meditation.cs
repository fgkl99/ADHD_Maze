using UnityEngine;

public class MusicSwitcher_Meditation : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip[] musicTracks; // Array to hold multiple music tracks
    private int currentTrackIndex = 0; // Keep track of the current music track

    void Start()
    {
        if (musicTracks.Length > 0)
        {
            audioSource.clip = musicTracks[currentTrackIndex];
            audioSource.Play();
        }
    }

    public void PlayNextTrack()
    {
        if (musicTracks.Length > 1)
        {
            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length; // Cycle through tracks
            audioSource.clip = musicTracks[currentTrackIndex];
            audioSource.Play();
        }
    }

    public void PlayPreviousTrack()
    {
        if (musicTracks.Length > 1)
        {
            currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Length) % musicTracks.Length; // Go backwards in the array
            audioSource.clip = musicTracks[currentTrackIndex];
            audioSource.Play();
        }
    }
}