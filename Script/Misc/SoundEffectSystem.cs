using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectSystem : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] soundEffects; // Array of sound effects
    private List<AudioSource> audioSources = new List<AudioSource>(); // List of AudioSource components

    void Start()
    {
        // Create multiple AudioSource components
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
    }

    // Method to play a sound effect
    public void PlaySoundEffect(int soundEffectIndex)
    {
        // Check if the sound effect index is valid
        if (soundEffectIndex >= 0 && soundEffectIndex < soundEffects.Length)
        {
            // Find an available AudioSource component
            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    // Play the sound effect
                    audioSource.clip = soundEffects[soundEffectIndex];
                    audioSource.Play();
                    return;
                }
            }

            // If all AudioSource components are busy, create a new one
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.clip = soundEffects[soundEffectIndex];
            newAudioSource.Play();
            audioSources.Add(newAudioSource);
        }
        else
        {
            Debug.LogError("Invalid sound effect index");
        }
    }
}