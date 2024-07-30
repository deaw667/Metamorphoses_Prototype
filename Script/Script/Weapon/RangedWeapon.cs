using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedWeapon : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    public float scaleX;
    public float scaleY;
    public float scaleZ;
    public float ShakeTime;
    public float ShakeIntensity;
    public GameObject bulletPrefab;
    public float BulletSpeed;
    public bool isBoltLoaded;
    public bool isShootingOrReload;
    public int MaxAmmoAmount;
    public int currentAmmoAmount;
    public Item AmmoType;
    public Text ammoText;
    public float muzzleTime = 0.1f;
    private SoundEffectSystem soundEffectSystem;

    
    [SerializeField]
    private AudioClip[] WeaponSoundEffects; // Array of sound effects
    private List<AudioSource> WeaponAudioSources = new List<AudioSource>(); // List of AudioSource components

    private GameObject slashAnim;
    public GameObject MuzzleFlashLight;

    private void Awake() 
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    void Start()
    {
        // Create multiple AudioSource components
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            WeaponAudioSources.Add(audioSource);
        }
        soundEffectSystem = GameObject.FindObjectOfType<SoundEffectSystem>();
        UpdateAmmoText();
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update() 
    {
        MouseFollowWithOffset();
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentAmmoAmount < MaxAmmoAmount && !isShootingOrReload)
            {
                PlayWeaponSoundEffect(2);
                myAnimator.SetTrigger("AmmoReload");
            }
            else
            {
                Debug.Log("You are Shooting");
            }
        }
    }

    private void UpdateAmmoText()
    {
        ammoText.text = $"Ammo: {currentAmmoAmount} / {MaxAmmoAmount}";
    }

    
    // Method to play a sound effect
    private void PlayWeaponSoundEffect(int soundEffectIndex)
    {
        // Check if the sound effect index is valid
        if (soundEffectIndex >= 0 && soundEffectIndex < WeaponSoundEffects.Length)
        {
            // Find an available AudioSource component
            foreach (AudioSource audioSource in WeaponAudioSources)
            {
                if (!audioSource.isPlaying)
                {
                    // Play the sound effect
                    audioSource.clip = WeaponSoundEffects[soundEffectIndex];
                    audioSource.Play();
                    return;
                }
            }

            // If all AudioSource components are busy, create a new one
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.clip = WeaponSoundEffects[soundEffectIndex];
            newAudioSource.Play();
            WeaponAudioSources.Add(newAudioSource);
        }
        else
        {
            Debug.LogError("Invalid sound effect index");
        }
    }

    private void Attack() 
    {
        if (isBoltLoaded && currentAmmoAmount > 0 && !isShootingOrReload)
        {
            myAnimator.SetTrigger("Attack");
            PlayWeaponSoundEffect(0);
            currentAmmoAmount -= 1;
            MuzzleFlashLight.SetActive(true);
            StartCoroutine(DisableMuzzleFlashLight());
            CinemachineShake.instance.ShakeCamera(ShakeIntensity,ShakeTime);
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;

            // Instantiate bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            bullet.transform.localScale = new Vector3(0f, 0.4f, 0.4f);
            // Calculate direction to mouse position
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);
            Vector3 direction = mousePos - playerScreenPoint;
            direction.z = 0; // Ignore z-axis for 2D rotation

            // Set bullet velocity
            bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * BulletSpeed;

            // Rotate bullet to face mouse position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            isBoltLoaded = false;
            isShootingOrReload = true;
        }
        else if (!isBoltLoaded && currentAmmoAmount > 0 && !isShootingOrReload)
        {
            PlayWeaponSoundEffect(1);
            myAnimator.SetTrigger("Reloading");
            isShootingOrReload = true;
        }
        else
        {
            Debug.Log("Maybe You are Out of Ammo");
        }
    }

    private IEnumerator DisableMuzzleFlashLight()
    {
        yield return new WaitForSeconds(muzzleTime);
        MuzzleFlashLight.SetActive(false);
    }

    private bool CanReload()
    {
        bool AmmoInInventory = Inventory.instance.ContainsItemAmount(AmmoType.name, 1);

        if (!AmmoInInventory)
        {
            return false;
        }

        return true;
    }

    public void DoneAttackingAnimEvent()
    {

    }

    public void ReloadedAmmo()
    {
        while (currentAmmoAmount < MaxAmmoAmount && CanReload())
        {
            Inventory.instance.RemoveItemThatAmount(AmmoType.name, 1);
            currentAmmoAmount += 1;
        }

        if (currentAmmoAmount >= MaxAmmoAmount)
        {
            Debug.Log("Ammo fully reloaded!");
        }
        else
        {
            Debug.Log("Out of Ammo");
        }
        //playerController.RemoveItem(AmmoType, MaxAmmoAmount - currentAmmoAmount);
    }

    public void BoltLoaded()
    {
        // Called when the bolt animation is complete
        isBoltLoaded = true;
    }

    public void EndAttackingAnim()
    {
        ResetToEntryState();
        isShootingOrReload = false;
        UpdateAmmoText();
    }

    public void ResetToEntryState()
    {
        // Reset the animator triggers
        myAnimator.ResetTrigger("Attack");
        myAnimator.ResetTrigger("Reloading");
        myAnimator.ResetTrigger("AmmoReload");

        // Reset the character's rotation and scale
        activeWeapon.transform.rotation = Quaternion.identity;
        activeWeapon.transform.localScale = new Vector3(1, 1, 1);

        // Reset any other state that needs to be reset
    }

    public void BackToBasic() 
    {
        isBoltLoaded = true;
        myAnimator.SetTrigger("BackToBasic");
    }


    public void SwingUpFlipAnimEvent() 
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft) 
        { 
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent() 
    {
        if (slashAnim!= null)
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void MouseFollowWithOffset() 
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        Vector3 direction = mousePos - playerScreenPoint;
        direction.z = 0; // Ignore z-axis for 2D rotation

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (playerController.FacingLeft) 
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle - 180);
            activeWeapon.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        } 
        else 
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            activeWeapon.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
    }
}