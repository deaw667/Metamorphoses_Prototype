using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class MoveZone : MonoBehaviour
{
    public GameObject HomeZone;
    public GameObject HomeZone2;
    public GameObject ForestZone1;
    public GameObject ForestZone2;
    public GameObject ForestZone3;
    public GameObject ForestZone4;
    public GameObject ForestZone5;
    public GameObject ForestZone6;

    public GameObject VirtualCam;

    public GameObject HomeZoneCamCol;
    public GameObject HomeZoneCamCol2;
    public GameObject ForestZone1CamCol;
    public GameObject ForestZone2CamCol;
    public GameObject ForestZone3CamCol;
    public GameObject ForestZone4CamCol;
    public GameObject ForestZone5CamCol;
    public GameObject ForestZone6CamCol;

    public int BGSong;

    public GameObject BlackScreen;

    public int Teleportto;
    private GameObject[] zones;
    private GameObject[] zonesCamCol;
    private SoundEffectSystem soundEffectSystem;
    public bool isPortalToForest;
    public bool isTutorialPortal;
    public bool isPortalOne;

    // Start is called before the first frame update
    void Start()
    {
        soundEffectSystem = GameObject.FindObjectOfType<SoundEffectSystem>();
        zones = new GameObject[] { HomeZone, HomeZone2, ForestZone1, ForestZone2, ForestZone3, ForestZone4, ForestZone5, ForestZone6 };
        zonesCamCol = new GameObject[] { HomeZoneCamCol, HomeZoneCamCol2, ForestZone1CamCol, ForestZone2CamCol, ForestZone3CamCol, ForestZone4CamCol, ForestZone5CamCol, ForestZone6CamCol };
    }

    // Update is called once per frame
    void Update()
    {
        if(ClockScript.instance.currentTime == 340f && isPortalToForest && isPortalOne)
        {
            Teleportto = Random.Range(2, 4);
            Debug.Log("RandomForestZone");
        }
        else if (ClockScript.instance.currentTime == 340f && isPortalToForest && !isPortalOne)
        {
            Teleportto = Random.Range(5, 7);
            Debug.Log("RandomForestZone");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.tag == "Player") 
        {
            // Freeze the player
            PlayerController.Instance.FreezePlayer(true);
            soundEffectSystem.PlaySoundEffect(0);

            // Wait for 2 seconds
            StartCoroutine(UnfreezeAndTeleport(other));
        }
    }

    private IEnumerator UnfreezeAndTeleport(Collider2D other)
    {
        // Get the BlackScreen UI Image
        Image blackScreenImage = BlackScreen.GetComponent<Image>();
        BlackScreen.SetActive(true);

        // Animate the BlackScreen alpha value to 1 over 2 seconds
        float timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / 2f);
            Color color = blackScreenImage.color;
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }

        // Teleport the player
        GameObject targetZone = zones[Teleportto];
        other.transform.position = targetZone.transform.position;
        BackgroundMusic.instance.ChangeSong(BGSong);

        // Get the Cinemachine Confiner 2D component
        CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();

        // Change the Bounding Shape 2D to the corresponding zone's camera collider
        confiner.m_BoundingShape2D = zonesCamCol[Teleportto].GetComponent<PolygonCollider2D>();

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        PlayerController.Instance.FreezePlayer(false);

        // Animate the BlackScreen alpha value back to 0 over 2 seconds
        timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / 2f);
            Color color = blackScreenImage.color;
            color.a = alpha;
            blackScreenImage.color = color;
            yield return null;
        }
        if(isTutorialPortal)
        {
            TutorialSystem.instance.ClosedPortal();
        }
        BlackScreen.SetActive(false);

        // Unfreeze the player
    }
}