using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public GameObject HealthBarMax;
    public GameObject HealthBar;
    private List<GameObject> MaxedhealthBars;
    private List<GameObject> healthBars;
    public GameObject healthBarPrefab;
    public GameObject HeartPrefab;
    public GameObject DeathPrefab;
    //public int currentHealth { get; private set; }
    private Flash flash;
    private GameObject lastDamagedHealthBar;
    public Collider2D boxCollider;

    public bool PlayedTutorial = false;

    #region singleton
    public static PlayerHealth instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion


    void Start()
    {
        currentHealth = startingHealth;
        flash = GetComponent<Flash>();
        StartInstantiateHealthBars();
        MaxedhealthBars = new List<GameObject>();
        healthBars = new List<GameObject>();

        foreach (Transform child in HealthBarMax.transform)
        {
            MaxedhealthBars.Add(child.gameObject);
        }

        foreach (Transform child in HealthBar.transform)
        {
            healthBars.Add(child.gameObject);
        }
    }

    void Update()
    {
        
    }

    public void UpdatePlayerHealth()
    {
        // Destroy all child objects of HealthBar
        foreach (Transform child in HealthBar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate new heart icons based on current health
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject hearticon = Instantiate(HeartPrefab, HealthBar.transform);
            hearticon.SetActive(true);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(flash.FlashRoutine());
        DeleteHeart(damage);
    }

    public void UseWerewolfTeleport()
    {
        GameObject player = GameObject.Find("Player");
        if (player!= null)
        {
            GameManager.instance.MoveToWereWolfLocation(player);
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene.");
        }
    }

    public void HealingPlayer(int healamount)
    {
        for (int i = 0; i < healamount; i++)
        {
            if(currentHealth < startingHealth)
            {
                currentHealth += 1;
                AddHeart(1);
            }
        }
    }

    public void UpdateMaxHealth(int healthamount)
    {
        if(startingHealth < 10)
        {
            startingHealth += healthamount;
            GameObject healthicon = Instantiate(healthBarPrefab, HealthBarMax.transform);
            healthicon.SetActive(true);
        }
        else
        {
            Debug.Log("That is the most your body can handle");
        }
    }

    public void ItemHealingPlayer(int healamount)
    {
        PlayerHealth.instance.HealingPlayer(healamount);
    }

    public void ItemMaxHealth(int healthamount)
    {
        PlayerHealth.instance.UpdateMaxHealth(healthamount);
    }
    

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && boxCollider.IsTouching(other))
        {
            TakeDamage(1);
        }
    }
    
    private void StartInstantiateHealthBars()
    {
        for (int i = 0; i < startingHealth; i++)
        {
            GameObject healthicon = Instantiate(healthBarPrefab, HealthBarMax.transform);
            healthicon.SetActive(true);
        }
        for (int i = 0; i < startingHealth; i++)
        {
            GameObject hearticon = Instantiate(HeartPrefab, HealthBar.transform);
            hearticon.SetActive(true);
        }
    }


    public void DeleteHeart(int damageamount)
    {
        for (int i = 0; i < damageamount; i++)
        {
            if (HealthBar.transform.childCount > 0)
            {
                Debug.Log("Delete one");
                GameObject heartIcon = HealthBar.transform.GetChild(0 + i).gameObject;
                Object.Destroy(heartIcon);
            }
        }
    }

    public void AddHeart(int healingamout)
    {
        for (int i = 0; i < healingamout; i++)
        {
            GameObject hearticon = Instantiate(HeartPrefab, HealthBar.transform);
            hearticon.SetActive(true);
        }
    }
}
