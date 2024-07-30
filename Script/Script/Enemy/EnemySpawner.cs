using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region singleton

    public static EnemySpawner instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #endregion


    // The array of enemy prefabs to instantiate
    public GameObject[] enemyPrefabs;

    // The maximum number of enemies to spawn
    public int maxEnemies = 3;

    [SerializeField]
    // List to store the instantiated enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
    }

    // Spawn random enemies until maxEnemies is reached
    public void SpawnEnemies()
    {
        // Choose a random enemy prefab
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // Instantiate the enemy at a random position around this game object (2D)
            Vector2 randomPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            randomPosition += (Vector2)transform.position;
            GameObject enemyInstance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            // Add the instantiated enemy to the list
            spawnedEnemies.Add(enemyInstance);
        }
    }

    // Destroy all instantiated enemies
    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }
}