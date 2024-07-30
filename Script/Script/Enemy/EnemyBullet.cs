using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject ExlodePrefab;
    void Start()
    {
        // Start a coroutine to destroy the game object after 10 seconds
        StartCoroutine(DestroyAfterDelay(10.0f));
    }

    // Coroutine to destroy the game object after a delay
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    // Called when the game object collides with another game object
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Sir I got Shot!");
        // If the game object collides with a player, destroy it
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(ExlodePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            PlayerHealth.instance.TakeDamage(1);
        }
    }
}