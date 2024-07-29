using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameObject Player;
    public GameObject EscTab;

    public void RestartButtonClicked()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Player.SetActive(true);
        PlayerHealth.instance.HealingPlayer(2);
        EscTab.SetActive(false);
        GameDataManager.instance.LoadGame();
    }
}