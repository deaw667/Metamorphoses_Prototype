using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{

    #region singleton

    public static Inventory instance;
    public static Inventory MainObject;
    public Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    public int maxcapacity;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        onItemChange.Invoke();
    }

    #endregion

    public delegate void OnItemChange();
    public OnItemChange onItemChange = delegate {};

    public List<Item> inventoryItemList = new List<Item>();
    public Item item;

    public List<Item> hotbarItemList = new List<Item>();
    public List<Item> ArmorItemList = new List<Item>();
    public HotbarController hotbarController;
    public ArmorSlot armorslot;

    public void SwitchHotbarInventory(Item item)
    {
        //inventory to hotbar, CHECK if we have enaugh space
        foreach(Item i in inventoryItemList)
        {
            if(i == item)
            {
                if(hotbarItemList.Count >= hotbarController.HotbarSlotSize)
                {
                    Debug.Log("No more slots available in hotbar");
                }
                else
                {
                    hotbarItemList.Add(item);
                    inventoryItemList.Remove(item);
                    onItemChange.Invoke();
                }
                return;
            }
        }

        //hotbar to inventory
        foreach(Item i in hotbarItemList)
        {
            if( i == item)
            {
                hotbarItemList.Remove(item);
                inventoryItemList.Add(item);
                onItemChange.Invoke();
                return;
            }
        }

    }

    public void SwitchArmorInventory(Item item)
    {
        // Check if item is an armor
        if (item is ArmorItem armorItem)
        {
            bool isAlreadyEquipped = false;
            // Check if there is already an item with the same type in the ArmorItemList
            if (ArmorItemList.Any(i => i is ArmorItem && ((ArmorItem)i).itemType == armorItem.itemType))
            {
                Debug.Log("Dump");
                isAlreadyEquipped = true;
                return;
            }
            // inventory to armor, CHECK if we have enough space
            foreach (Item i in inventoryItemList)
            {
                Debug.Log("int to Armor");
                if (i == item && i is ArmorItem armor)
                {
                    if (armorItem.itemType == armor.itemType)
                    {
                        if (ArmorItemList.Count >= armorslot.ArmorSlotSize)
                        {
                            Debug.Log("No more slots available in armor");
                        }
                        else if (isAlreadyEquipped)
                        {
                            Debug.Log("There is already an item with the same type in the ArmorItemList");
                        }
                        else
                        {
                            Debug.Log("Added Armor");
                            ArmorItemList.Add(item);
                            inventoryItemList.Remove(item);
                            onItemChange.Invoke();
                        }
                        return;
                    }
                }
            }

            // armor to inventory
            foreach (Item i in ArmorItemList)
            {
                Debug.Log("Armor to inv");
                if (i == item && i is ArmorItem armor)
                {
                    Debug.Log("Armor to inv");
                    ArmorItemList.Remove(item);
                    inventoryItemList.Add(item);
                    onItemChange.Invoke();
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Item is not an armor");
        }
    }


    public void AddItem(Item item)
    {
        if (itemCounts.Count < maxcapacity)
        {
            string itemName = item.name;
            bool itemAlreadyExists = false;
            foreach (Item i in inventoryItemList)
            {
                if (i.name == itemName)
                {
                    itemAlreadyExists = true;
                    break;
                }
            }

            foreach (Item i in hotbarItemList)
            {
                if (i != null && i.name == itemName)
                {
                    itemAlreadyExists = true;
                    break;
                }
            }
            
            if (!itemAlreadyExists)
            {
                inventoryItemList.Add(item);
                if (itemCounts.ContainsKey(itemName))
                {
                    itemCounts[itemName]++;
                }
                else
                {
                    itemCounts.Add(itemName, 1);
                }
                onItemChange.Invoke();
                GameManager.instance.DisplayItemNotification(itemName);
            }
            else if (itemAlreadyExists && item.Isstackable == 0)
            {
                inventoryItemList.Add(item);
                if (itemCounts.ContainsKey(itemName))
                {
                    itemCounts[itemName]++;
                }
                else
                {
                    itemCounts.Add(itemName, 1);
                }
                onItemChange.Invoke();
                GameManager.instance.DisplayItemNotification(itemName);
            }
            else
            {
                GameManager.instance.DisplayItemNotification(itemName);
                // Item with the same name already exists, just increment the count
                itemCounts[itemName]++;
            }
        }
        else
        {
            Debug.Log("your inventory full");
        }
    }


    public void RemoveItem(Item item)
    {
        if (inventoryItemList.Contains(item))
        {
            inventoryItemList.Remove(item);
        }
        else if (hotbarItemList.Contains(item))
        {
            hotbarItemList.Remove(item);
        }
        else if (ArmorItemList.Contains(item))
        {
            ArmorItemList.Remove(item);
        }

        string itemName = item.name;
        if (itemCounts.ContainsKey(itemName))
        {
            itemCounts[itemName]--;
            if (itemCounts[itemName] <= 0)
            {
                itemCounts.Remove(itemName);
            }
        }

        onItemChange.Invoke();
    }
    
    /*public bool ContainsItem(string itemName, int amount)
    {
        int itemCounter = 0;

        foreach(Item i in inventoryItemList)
        {
            if(i.name == itemName)
            {
                itemCounter++;
            }
        }

        foreach (Item i in hotbarItemList)
        {
            if (i.name == itemName)
            {
                itemCounter++;
            }
        }

        
        foreach (Item i in ArmorItemList)
        {
            if (i.name == itemName)
            {
                itemCounter++;
            }
        }

        if (itemCounter >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    } */

    public bool ContainsItemAmount(string itemName, int amount)
    {
        if (itemCounts.ContainsKey(itemName))
        {
            return itemCounts[itemName] >= amount;
        }
        else
        {
            return false;
        }
    }

   /* public void RemoveItems(string itemName, int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            RemoveItemType(itemName);
        }
    } */




    //====================================================================//
    //====================================================================//
    public void RemoveItemThatAmount(string itemName, int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            RemoveItemTypeAmount(itemName);
        }
    }

    
    public void RemoveItemTypeAmount(string itemName)
    {
        foreach (Item i in inventoryItemList)
        {
            if (i.name == itemName)
            {
                //inventoryItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                    inventoryItemList.Remove(i);
                    onItemChange.Invoke();
                }
                return;
            }
        }

        foreach (Item i in hotbarItemList)
        {
            if (i.name == itemName)
            {
                //hotbarItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                    hotbarItemList.Remove(i);
                    onItemChange.Invoke();
                }
                return;
            }
        }

        foreach (Item i in ArmorItemList)
        {
            if (i.name == itemName)
            {
                //ArmorItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                    onItemChange.Invoke();
                }
                return;
            }
        }
    }

    //====================================================================//
    //====================================================================//






    //====================================================================//
    //====================================================================//
    
    public void RemoveItemAmount(Item item, int amount)
    {
        string itemName = item.name;
        if (itemCounts.ContainsKey(itemName))
        {
            itemCounts[itemName] -= amount;
            if (itemCounts[itemName] <= 0)
            {
                itemCounts.Remove(itemName);
                inventoryItemList.Remove(item);
                hotbarItemList.Remove(item);
            }
        }

        onItemChange.Invoke();
    }

    /*public void RemoveItemType(string itemName)
    {
        foreach (Item i in inventoryItemList)
        {
            if (i.name == itemName)
            {
                inventoryItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                }
                return;
            }
        } 

        foreach (Item i in hotbarItemList)
        {
            if (i.name == itemName)
            {
                hotbarItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                }
                return;
            }
        }

        foreach (Item i in ArmorItemList)
        {
            if (i.name == itemName)
            {
                ArmorItemList.Remove(i);
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                {
                    itemCounts.Remove(itemName);
                }
                return;
            }
        }
    } */

}
