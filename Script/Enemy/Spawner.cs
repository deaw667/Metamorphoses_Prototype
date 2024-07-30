using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region singleton

    public static Spawner instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #endregion
    public GameObject mageEnemyPrefab;
    public List<GameObject> currentmage;
    public float spawnRadius = 10f;
    public float spawnloop = 1f;
    public int spawnCount = 0;
    public int monsterperloop;

    private void Start()
    {
        StartCoroutine(SpawnMageEnemy());
    }

    private IEnumerator SpawnMageEnemy()
    {
        while (true)
        {
            // Check if the number of current mage enemies is less than the monsterperloop variable
            if (currentmage.Count < monsterperloop)
            {
                // Wait for 3 seconds before instantiating a new mageEnemyPrefab object
                yield return new WaitForSeconds(3f);

                // Instantiate a new mageEnemyPrefab object at the position of the spawner
                Instantiate(mageEnemyPrefab, transform.position, Quaternion.identity);

                // Add the new mage enemy to the currentmage list
                currentmage.Add(Instantiate(mageEnemyPrefab, transform.position, Quaternion.identity));
            }
            else
            {
                // Wait for 1 second before checking again
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void GetKilled()
    {
        Destroy(currentmage[0]);
        currentmage.RemoveAt(0);
    }
/*
    private void Start()
    {
        StartCoroutine(SpawnMageEnemy());
    }

    public void GetKilled()
    {
        spawnCount--;
    }

    private IEnumerator SpawnMageEnemy()
    {
        while (spawnCount < monsterperloop)
        {
            // Check if there is a game object within the spawn radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spawnRadius);
            bool gameObjectFound = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject!= gameObject && spawnCount >= 2)
                {
                    gameObjectFound = true;
                    break;
                }
            }

            // If no game object is found, wait for spawnloop seconds and then instantiate a new mageEnemyPrefab object
            if (!gameObjectFound)
            {
                yield return new WaitForSeconds(spawnloop);
                Instantiate(mageEnemyPrefab, transform.position, Quaternion.identity);
                spawnCount++;
            }
            else if(gameObjectFound && spawnCount < 2)
            {
                yield return new WaitForSeconds(spawnloop);
                Instantiate(mageEnemyPrefab, transform.position, Quaternion.identity);
                spawnCount++;
            }

            // Wait for 1 second before checking again
            yield return new WaitForSeconds(1f);
        }
    } */
}