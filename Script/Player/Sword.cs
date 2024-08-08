using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    public bool isSwordType1;
    public bool isSwordType2;

    private GameObject slashAnim;
    private bool isPlayingAnimation;

    [SerializeField]
    private AudioClip[] WeaponSoundEffects; // Array of sound effects
    private List<AudioSource> WeaponAudioSources = new List<AudioSource>(); // List of AudioSource components

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
        playerControls.Combat.Attack.started += _ => Attack();
        // Create multiple AudioSource components
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            WeaponAudioSources.Add(audioSource);
        }
    }

    private void Update() 
    {
        if(isSwordType1)
        {
            MouseFollowWithOffset();
        }
        if(isSwordType2)
        {
            MouseFollowWithOffset2();
        }
    }

    private void Attack() 
    {
        if(!isPlayingAnimation)
        {
            myAnimator.SetTrigger("Attack");
            isPlayingAnimation = true;
            weaponCollider.gameObject.SetActive(true);

            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
        }    
    }

    private void EndAttackingAnim()
    {
        isPlayingAnimation = false;
    }

    public void DoneAttackingAnimEvent() 
    {
        weaponCollider.gameObject.SetActive(false);
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


    public void SwingUpFlipAnimEvent() 
    {
        if (slashAnim != null) 
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (playerController.FacingLeft) 
            { 
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void SwingDownFlipAnimEvent() 
    {
        if (slashAnim != null) 
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    //#### Defualt
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
            activeWeapon.transform.localScale = new Vector3(-1, 1, 1);
            if (slashAnim!= null) 
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, angle - 180);
            }
        } 
        else 
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            activeWeapon.transform.localScale = new Vector3(1, 1, 1);
            if (slashAnim!= null) 
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    private void MouseFollowWithOffset2() 
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) 
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            if (slashAnim!= null) 
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, -180, angle);
            }
        } 
        else 
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (slashAnim!= null) 
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

}
