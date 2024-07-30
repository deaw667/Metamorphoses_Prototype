using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private Image healthBar; // Add a reference to the health bar UI image
    [SerializeField] private GameObject healthBarFamily; // Add a reference to the health bar UI image
    [SerializeField] private bool isBoss; // Whether this enemy is a boss or not

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    public List<GameObject> itemdrop;
    public GameObject dropposition;

    private void Awake() 
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() 
    {
        currentHealth = startingHealth;
        if (isBoss) 
        {
            //healthBarFamily.SetActive(true); // Show the health bar if this is a boss
        }
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(flash.FlashRoutine());
        if (isBoss) 
        {
            UpdateHealthBar(); // Update the health bar
        }
    }

    public void DetectDeath() 
    {
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
            Instantiate(itemdrop[Random.Range(0, itemdrop.Count)], dropposition.transform.position, Quaternion.identity);
            Spawner.instance.GetKilled();
            if (isBoss) 
            {
                healthBarFamily.SetActive(false); // Hide the health bar when the boss dies
            }
        }
    }

    private void UpdateHealthBar() 
    {
        float healthPercentage = (float)currentHealth / startingHealth;
        healthBar.fillAmount = healthPercentage;
    }
}
