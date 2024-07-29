using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private string saveFilePath;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/player_data.json";
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);

        GameData data = new GameData();
        data.currentHealth = PlayerHealth.instance.currentHealth;
        data.startingHealth = PlayerHealth.instance.startingHealth;
        data.PlayedTutorial = PlayerHealth.instance.PlayedTutorial;

        data.CurrentDayCount = ClockScript.instance.CurrentDayCount;

        data.itemCounts = Inventory.instance.itemCounts;
        data.inventoryItems = new List<string>();
        data.hotbarItems = new List<string>();
        data.armorItems = new List<string>();
        foreach (Item item in Inventory.instance.inventoryItemList)
        {
            data.inventoryItems.Add(item.name);
        }
        foreach (Item item in Inventory.instance.hotbarItemList)
        {
            data.hotbarItems.Add(item.name);
        }
        foreach (Item item in Inventory.instance.ArmorItemList)
        {
            data.armorItems.Add(item.name);
        }

        formatter.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);

            GameData data = formatter.Deserialize(file) as GameData;
            file.Close();

            PlayerHealth.instance.currentHealth = data.currentHealth;
            PlayerHealth.instance.startingHealth = data.startingHealth;
            PlayerHealth.instance.PlayedTutorial = data.PlayedTutorial;

            ClockScript.instance.CurrentDayCount = data.CurrentDayCount;
            ClockScript.instance.currentTime = 340;

            // Load inventory data
            Inventory.instance.itemCounts = data.itemCounts;
            Inventory.instance.inventoryItemList.Clear();
            Inventory.instance.hotbarItemList.Clear();
            Inventory.instance.ArmorItemList.Clear();
            foreach (string itemName in data.inventoryItems)
            {
                Item item = GameManager.instance.GetItemFromName(itemName);
                Inventory.instance.inventoryItemList.Add(item);
            }
            foreach (string itemName in data.hotbarItems)
            {
                Item item = GameManager.instance.GetItemFromName(itemName);
                Inventory.instance.hotbarItemList.Add(item);
            }
            foreach (string itemName in data.armorItems)
            {
                Item item = GameManager.instance.GetItemFromName(itemName);
                Inventory.instance.ArmorItemList.Add(item);
            }
            Inventory.instance.onItemChange.Invoke();
            PlayerHealth.instance.UpdatePlayerHealth();
            TutorialSystem.instance.UpdateCurrentTutorialState();
            Debug.Log("Loaded!!");
        }
        else
        {
            Debug.LogError("Where is save file??");
        }
    }

    [System.Serializable]
    private class GameData
    {
        public int currentHealth;
        public int startingHealth;
        public int CurrentDayCount;
        public bool PlayedTutorial;
        public Dictionary<string, int> itemCounts;
        public List<string> inventoryItems = new List<string>();
        public List<string> hotbarItems = new List<string>();
        public List<string> armorItems = new List<string>();
    }
}