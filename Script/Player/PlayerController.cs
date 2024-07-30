using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    // Singleton instance of the PlayerController
    public static PlayerController Instance;

    [SerializeField]
    private AudioClip[] soundEffects; // Array of sound effects
    private List<AudioSource> audioSources = new List<AudioSource>(); // List of AudioSource components

    // Player movement speed
    public float moveSpeed = 1f;
    public float defaultmovespeed;

    // Player controls
    private PlayerControls playerControls;

    // Player movement and rigidbody
    private Vector2 movement;
    private Rigidbody2D rb;


    // Player animator and sprite renderer
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    // Player stamina drain amount
    public float staminadrainamount;

    // Player facing direction
    private bool facingLeft = false;

    private void Awake() 
    {
        // Initialize singleton instance
        Instance = this;

        // Initialize player controls
        playerControls = new PlayerControls();

        // Get player rigidbody and animator components
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
    }

    private void OnEnable() 
    {
        // Enable player controls
        playerControls.Enable();
    }

    private void Update() 
    {
        // Handle player input and running
        PlayerInput();
    }

    private void FixedUpdate() 
    {
        // Adjust player facing direction and move player
        AdjustPlayerFacingDirection();
        Move();
       
    }

        // Method to play a sound effect
    public void PlayPlayerSoundEffect(int soundEffectIndex)
    {
        // Check if the sound effect index is valid
        if (soundEffectIndex >= 0 && soundEffectIndex < soundEffects.Length)
        {
            // Find an available AudioSource component
            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    // Play the sound effect
                    audioSource.clip = soundEffects[soundEffectIndex];
                    audioSource.Play();
                    return;
                }
            }

            // If all AudioSource components are busy, create a new one
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.clip = soundEffects[soundEffectIndex];
            newAudioSource.Play();
            audioSources.Add(newAudioSource);
        }
        else
        {
            Debug.LogError("Invalid sound effect index");
        }
    }

    private void OnRightFoot()
    {
        PlayPlayerSoundEffect(0);
    }

    private void OnLeftFoot()
    {
        PlayPlayerSoundEffect(1);
    }

    // This method is called every fixed frame rate frame and adjusts the player's facing direction and moves the player.
    private void PlayerInput() 
    {
        // Get player movement input
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
        // Check if the "Q" key is pressed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            moveSpeed = defaultmovespeed * 1.5f;
            Stamina.instance.DrainStamina(staminadrainamount);
        } 
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Am Dash");
            // Start dashing
            StartCoroutine(Dash());
        }
        else
        {
            moveSpeed = defaultmovespeed;
        }
    }

    // This method moves the player based on their movement input and movement speed.
    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            playerControls.Disable();
        }
        else
        {
            rb.isKinematic = false;
            playerControls.Enable();
        }
    }

    // This method adjusts the player's facing direction based on the position of the mouse cursor.
    private void AdjustPlayerFacingDirection() 
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) 
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        } 
        else 
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    // This public method reduces the player's movement speed by a specified amount.
    public void SlowDownPlayer(float amount)
    {
        moveSpeed -= amount;
    }

    private IEnumerator Dash()
    {
        // Increase the player's movement speed
        moveSpeed = defaultmovespeed * 2f;

        // Wait for a short duration (e.g. 0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Restore the player's movement speed
        moveSpeed = defaultmovespeed;
    }
}
