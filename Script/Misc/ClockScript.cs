using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public GameObject hourArrow;
    public Text timeText;
    public int timeSpeedMultiplier = 1; // Multiplier for time speed
    public float arrowSpeedMultiplier = 1f; // Multiplier for arrow speed
    public float startTime = 0f; // Initial time in seconds
    public int hours;
    public int minutes;
    public float gameTimeInHours;
    public float gameHour;
    public int CurrentDayCount;

    public float hourAngle = 0f;
    public float currentTime = 0f;

    #region singleton
    public static ClockScript instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeSpeedMultiplier;

        if (currentTime >= 1440)
        {
            currentTime = 0; // Reset currentTime to 0
        }

        if(currentTime == 0)
        {
            GameManager.instance.StartCoroutine(GameManager.instance.UnfreezeAndTeleport());
            CurrentDayCount += 1;
            currentTime = 340; // Reset currentTime to 0
            // Find all EnemySpawner instances in the scene
            EnemySpawner[] enemySpawners = FindObjectsOfType<EnemySpawner>();
            GameDataManager.instance.SaveGame();

            // Destroy all enemies spawned by each EnemySpawner instance
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                enemySpawner.DestroyAllEnemies();
                enemySpawner.SpawnEnemies();
            }
        }


        // Calculate the game time in hours
        gameTimeInHours = currentTime / 60f; // 1 minute in real life = 24 hours in game

        // Calculate the hour in game-time
        gameHour = gameTimeInHours % 24;

        // Calculate the angle for the hour hand
        hourAngle = gameHour * 15f; // 15 degrees per hour for a 24-hour clock

        // Rotate the hour hand with the arrow speed multiplier
        hourArrow.transform.localEulerAngles = new Vector3(0f, 0f, -hourAngle * arrowSpeedMultiplier + 180f);

        // Update the time text
        hours = (int)gameHour;
        minutes = (int)((gameTimeInHours % 1) * 60);
        timeText.text = $"{hours:00}:{minutes:00}";
    }
}
