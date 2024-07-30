using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialSystem : MonoBehaviour
{
    #region singleton
    public static TutorialSystem instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public GameObject WalkTutorial;
    public GameObject CraftTutorial;
    public GameObject FightTutorial;
    public GameObject TutorialTab;
    public GameObject HomePortal;
    public GameObject PortalTab;
    public GameObject NewplayerPosition;
    public GameObject VirtualCam;
    public GameObject Player;
    public GameObject TutorialZoneCol;

    public GameObject enemyspawned;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerHealth.instance.PlayedTutorial)
        {
            TutorialTab.SetActive(true);
            OpenWalktutorial();
            Player.transform.position = NewplayerPosition.transform.position;
            CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = TutorialZoneCol.GetComponent<PolygonCollider2D>();
        }
        else
        {
            TutorialTab.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyspawned == null)
        {
            OpenPortal();
        }
    }

    public void UpdateCurrentTutorialState()
    {
        if(!PlayerHealth.instance.PlayedTutorial)
        {
            TutorialTab.SetActive(true);
            OpenWalktutorial();
            CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = TutorialZoneCol.GetComponent<PolygonCollider2D>();
            Player.transform.position = NewplayerPosition.transform.position;
        }
        else
        {
            TutorialTab.SetActive(false);
        }
    }

    public void StartCutScene()
    {

    }

    
    public void PlayerWalked()
    {
        WalkTutorial.SetActive(false);
        OpenCraftingTutorial();
    }

    public void OpenWalktutorial()
    {
        BackgroundMusic.instance.ChangeSong(1);
        WalkTutorial.SetActive(true);
    }



    public void PlayerCrafted()
    {
        CraftTutorial.SetActive(false);
        enemyspawned.SetActive(true);
        OpenFightingTutorial();
    }

    public void OpenCraftingTutorial()
    {
        CraftTutorial.SetActive(true);
    }

    

    public void PlayerKilled()
    {
        FightTutorial.SetActive(false);
    }

    public void OpenFightingTutorial()
    {
        FightTutorial.SetActive(true);
    }

    public void OpenPortal()
    {
        FightTutorial.SetActive(false);
        HomePortal.SetActive(true);
        PortalTab.SetActive(true);
    }

    public void ClosedPortal()
    {
        PlayerHealth.instance.PlayedTutorial = true;
        TutorialTab.SetActive(false);
        HomePortal.SetActive(false);
        PortalTab.SetActive(false);
    }
}
