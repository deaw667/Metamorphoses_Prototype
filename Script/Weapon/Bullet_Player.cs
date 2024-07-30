using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player : MonoBehaviour
{
    public float animationDuration = 0.5f;
    public float targetScaleX = 2f;
    public GameObject ExlodePrefab;
    [SerializeField] private int damageAmount = 2;

    private Vector3 initialScale;
    private float animationStartTime;
    private SoundEffectSystem soundEffectSystem;

    void Start()
    {
        soundEffectSystem = GameObject.FindObjectOfType<SoundEffectSystem>();
        StartCoroutine(DestroyAfterDelay(10.0f));
        initialScale = transform.localScale;
        animationStartTime = Time.time;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<EnemyHealth>()) 
        {
            soundEffectSystem.PlaySoundEffect(4);
            Instantiate(ExlodePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }

    void Update()
    {
        float t = (Time.time - animationStartTime) / animationDuration;
        t = Mathf.Clamp01(t); // ensure t is between 0 and 1

        float scaleX = Mathf.Lerp(0f, targetScaleX, t);
        transform.localScale = new Vector3(scaleX, initialScale.y, initialScale.z);
    }
}
