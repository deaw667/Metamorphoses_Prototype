using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    #region singleton
    public static BackgroundMusic instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    // The AudioSource component attached to this GameObject
    public AudioSource audioSource;

    // The current song being played
    private AudioClip currentSong;

    // A list of available songs
    public List<AudioClip> songs = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        // Play the first song in the list by default
        if (songs.Count > 0)
        {
            PlaySong(songs[0]);
        }
        TutorialSystem.instance.UpdateCurrentTutorialState();
    }

    public void ChangeSong(int songnum)
    {
        PlaySong(songs[songnum]);
    }

    public void PlaySong(AudioClip song)
    {
        // Stop the current song
        audioSource.Stop();

        // Set the current song
        currentSong = song;

        // Play the new song
        audioSource.clip = currentSong;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Play a random song from the list
    public void PlayRandomSong()
    {
        // Choose a random song from the list
        int randomIndex = Random.Range(0, songs.Count);
        AudioClip randomSong = songs[randomIndex];

        // Play the random song
        PlaySong(randomSong);
    }
}