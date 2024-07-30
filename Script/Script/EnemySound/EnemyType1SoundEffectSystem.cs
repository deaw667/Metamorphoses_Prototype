using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1SoundEffectSystem : MonoBehaviour
{
    public AudioClip[] enemyType1SoundEffects; // Array of enemy type 1 sound effects
    private AudioSource[] enemyType1AudioSources; // Array of AudioSources for enemy type 1

    void Start()
    {
        // Create an AudioSource for each enemy type 1 sound effect
        enemyType1AudioSources = new AudioSource[enemyType1SoundEffects.Length];
        for (int i = 0; i < enemyType1SoundEffects.Length; i++)
        {
            enemyType1AudioSources[i] = gameObject.AddComponent<AudioSource>();
            enemyType1AudioSources[i].playOnAwake = false;
        }
    }

    // Method to play an enemy type 1 sound effect
    public void PlayEnemyType1SoundEffect(int soundEffectIndex)
    {
        // Check if the sound effect index is valid
        if (soundEffectIndex >= 0 && soundEffectIndex < enemyType1SoundEffects.Length)
        {
            // Play the sound effect on the corresponding AudioSource
            enemyType1AudioSources[soundEffectIndex].clip = enemyType1SoundEffects[soundEffectIndex];
            enemyType1AudioSources[soundEffectIndex].Play();
        }
        else
        {
            Debug.LogError("Invalid enemy type 1 sound effect index");
        }
    }
}