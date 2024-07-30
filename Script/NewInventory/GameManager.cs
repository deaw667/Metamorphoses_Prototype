using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public List<Item> itemList = new List<Item>();
    public List<Item> craftingRecipes = new List<Item>();
    public List<Item> allItems = new List<Item>();

    public Transform canvas;
    public GameObject itemInfoPrefab;
    public GameObject ItemNotifyPrefab;
    private GameObject currentItemInfo = null;
    private GameObject currentItemNotify = null;

    public GameObject WereWolfLocation;
    public GameObject BosszonesCamCol;

    public Transform mainCanvas;
    public Transform hotbarTransform;
    public Transform inventoryTransform;
    public Transform ArmorTransform;
    public Transform ItemNotifyTransform;
    public Transform PlayerTranform;
    public GameObject restartUi;
    public GameObject VirtualCam;
    public GameObject BossBar;
    public GameObject HomeZoneCamCol;
    public GameObject PlayerHomePosition;
    public GameObject Player;
    public int BackGroundSong;
    public int HomeSong;

    public GameObject BlackScreen;


    private void Start()
    {
        Item newItem = itemList[Random.Range(0, itemList.Count)];
        Inventory.instance.AddItem(newItem);
        /*if(!PlayerHealth.instance.PlayedTutorial)
        {
            CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = TutorialZoneCol.GetComponent<PolygonCollider2D>();
        }*/
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Item newItem = itemList[Random.Range(0, itemList.Count)];

            Inventory.instance.AddItem(Instantiate(newItem));
        }

        if (PlayerHealth.instance.currentHealth <= 0)
        {
            restartUi.SetActive(true);
        }
    }

    public void MoveToWereWolfLocation(GameObject targetObject)
    {
        StartCoroutine(UnfreezeAndTeleport(targetObject));
    }


    private IEnumerator UnfreezeAndTeleport(GameObject targetObject)
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
        targetObject.transform.position = WereWolfLocation.transform.position;
        BossBar.SetActive(true);
        BackgroundMusic.instance.ChangeSong(3);
        // Get the Cinemachine Confiner 2D component
        CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();

        // Change the Bounding Shape 2D to the corresponding zone's camera collider
        confiner.m_BoundingShape2D = BosszonesCamCol.GetComponent<PolygonCollider2D>();

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
        BlackScreen.SetActive(false);

        // Unfreeze the player
    }

    

    public Item GetItemFromName(string itemName)
    {
        // Find the item with the matching name
        Item item = allItems.Find(i => i.name == itemName);
        return item;
    }

    public void SwapWeapon(GameObject weapon)
    {
        foreach (Transform child in PlayerTranform)
        {
            if (child.gameObject.CompareTag("Weapon"))
            {
                Destroy(child.gameObject);
            }
        }
        // Check if the currentWeapon already exists as a child of PlayerTransform
        Transform weaponTransform = PlayerTranform.Find(weapon.name);
        if (weaponTransform!= null)
        {
            Destroy(weaponTransform.gameObject);
            // If the weapon already exists, destroy it
            Debug.Log("You already equip it!!");
        }
            Instantiate(weapon, PlayerTranform);
        // Instantiate the new weapon as a child of PlayerTransform
    }

    public void OnStatItemuUse(StatItemType itemType, int amount)
    {
        Debug.Log("Consuming " + itemType + " Add amount: " + amount);
    }

    public void DisplayItemInfo(string itemName, string itemDescription, Vector2 buttonPos)
    {
        if(currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }

        buttonPos.x -= 180;
        buttonPos.y += 100;

        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, canvas);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
    }
        
    
    public void DisplayItemNotification(string itemName)
    {
        currentItemNotify = Instantiate(ItemNotifyPrefab, ItemNotifyTransform);
        currentItemNotify.GetComponent<ItemNotify>().SetUpNotify(itemName);
      //  currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, canvas);
       // currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
       Destroy(currentItemNotify, 2f);
    }

    public void DestroyItemInfo()
    {
        if(currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }
    }

    public IEnumerator UnfreezeAndTeleport()
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
        GameObject targetZone = PlayerHomePosition;
        Player.transform.position = targetZone.transform.position;
        BackgroundMusic.instance.ChangeSong(HomeSong);

        // Get the Cinemachine Confiner 2D component
        CinemachineConfiner2D confiner = VirtualCam.GetComponent<CinemachineConfiner2D>();

        // Change the Bounding Shape 2D to the corresponding zone's camera collider
        confiner.m_BoundingShape2D = HomeZoneCamCol.GetComponent<PolygonCollider2D>();

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
        
        BlackScreen.SetActive(false);

        // Unfreeze the player
    }

}
