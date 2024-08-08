using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemAmountText;
    public GameObject usetab;
    private Item item;
    public bool isBeingDraged = false;

    public Item Item => item;

    private void Start()
    {
        itemAmountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnUseMenu()
    {
        usetab.SetActive(true);
    }

    public void OnExitUseMenu()
    {
        usetab.SetActive(false);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        UpdateItemAmountText();
        //GameManager.instance.DisplayItemNotification();
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
     //   itemAmountText.text = "";
    }

    private void UpdateItemAmountText()
    {

    }

    public void UseItem()
    {
        if (item == null || isBeingDraged == true) return;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Debug.Log("Trying to switch");
            Inventory.instance.SwitchHotbarInventory(item);
        }
        else
        {
            item.Use();
        }
    }   
    
    public void DestroySlot()
    {
        Destroy(gameObject);
    }

    public void OnRemoveButtonClicked()
    {
        if(item != null)
        {
            Inventory.instance.RemoveItem(item);
        }
    }

    private void Update()
    {
        icon.color = icon.sprite == null ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        //itemAmountText.color = icon.sprite == null ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        
        if (item != null && item.Isstackable == 0)
        {
            itemAmountText.color = new Color(itemAmountText.color.r, itemAmountText.color.g, itemAmountText.color.b, 0);
        }
        else
        {
            itemAmountText.color = new Color(itemAmountText.color.r, itemAmountText.color.g, itemAmountText.color.b, 1);
            itemAmountText.color = icon.sprite == null ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        }

        if (item != null && Inventory.instance != null)
        {
            string itemName = item.name;
            if (Inventory.instance.itemCounts.TryGetValue(itemName, out int itemCount))
            {
                itemAmountText.text = itemCount > 1 ? itemCount.ToString() : "";
            }
            else
            {
                itemAmountText.text = "";
            }
        }
    }

    
    public void OnCursorEnter()
    {
        if (item == null || isBeingDraged == true) return;

        //display item info
        GameManager.instance.DisplayItemInfo(item.name, item.GetItemDescription(), transform.position);
    }

    public void OnCursorExit()
    {
        if (item == null) return;

        GameManager.instance.DestroyItemInfo();
    }
}
