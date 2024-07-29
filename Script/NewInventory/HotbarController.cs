using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HotbarController class controls the functionality of the player's hotbar,
// which can hold up to 6 items at a time.
public class HotbarController : MonoBehaviour
{
    // HotbarSlotSize is a property that returns the number of slots in the hotbar.
    public int HotbarSlotSize => gameObject.transform.childCount;

    // hotbarSlots is a list of ItemSlot objects that represent each slot in the hotbar.
    public List<ItemSlot> hotbarSlots = new List<ItemSlot>();

    // hotbarKeys is an array of KeyCode objects that correspond to the number keys 1-6 on the keyboard.
    KeyCode[] hotbarKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    // Start is called when the script is first loaded.
    private void Start()
    {
        // Set up the hotbar slots by getting a reference to each ItemSlot object
        // and adding it to the hotbarSlots list.
        SetUpHotbarSlots();

        // Subscribe to the Inventory's onItemChange event so that the hotbar UI can be updated
        // whenever the player's inventory changes.
        Inventory.instance.onItemChange += UpdateHotbarUI;
    }

    // Update is called every frame.
    private void Update()
    {
        // Loop through each hotbar key.
        for (int i = 0; i < hotbarKeys.Length; i++)
        {
            // If the player presses down on a hotbar key...
            if (Input.GetKeyDown(hotbarKeys[i]))
            {
                // Use the item in the corresponding hotbar slot.
                hotbarSlots[i].UseItem();

                // Exit the function to prevent the hotbar UI from being updated multiple times.
                return;
            }
        }
    }

    // UpdateHotbarUI is called whenever the player's inventory changes.
    private void UpdateHotbarUI()
    {
        // Get the number of items currently being held in the hotbar.
        int currentUsedSlotCount = Inventory.instance.hotbarItemList.Count;

        // Loop through each slot in the hotbar.
        for (int i = 0; i < HotbarSlotSize; i++)
        {
            // If the current slot is being used...
            if (i < currentUsedSlotCount)
            {
                // Add the corresponding item to the slot.
                hotbarSlots[i].AddItem(Inventory.instance.hotbarItemList[i]);
            }
            // If the current slot is not being used...
            else
            {
                // Clear the slot.
                hotbarSlots[i].ClearSlot();
            }
        }
    }

    // SetUpHotbarSlots is called in the Start function to set up the hotbar slots.
    private void SetUpHotbarSlots()
    {
        // Loop through each child object of the hotbar.
        for (int i = 0; i < HotbarSlotSize; i++)
        {
            // Get a reference to the ItemSlot object in the current child.
            ItemSlot slot = gameObject.transform.GetChild(i).GetComponent<ItemSlot>();

            // Add the ItemSlot object to the hotbarSlots list.
            hotbarSlots.Add(slot);
        }
    }
}