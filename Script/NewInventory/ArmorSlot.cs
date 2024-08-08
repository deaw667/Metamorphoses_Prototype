using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlot : MonoBehaviour
{
    public int ArmorSlotSize => gameObject.transform.childCount;
    public List<ItemSlot> ArmorSlots = new List<ItemSlot>();


    private void Start()
    {
        SetUpArmorSlots();
        Inventory.instance.onItemChange += UpdateArmorUI;
    }

    private void Update()
    {

    }


    private void UpdateArmorUI()
    {
        int currentUsedSlotCount = Inventory.instance.ArmorItemList.Count;
        for(int i = 0; i < ArmorSlotSize; i++)
        {
            if(i < currentUsedSlotCount)
            {
                ArmorItem item = (ArmorItem)Inventory.instance.ArmorItemList[i];
                if(item.itemType == ArmorItemType.Head)
                {
                    ArmorSlots[0].AddItem(item);
                    Debug.Log("it a helmet");
                }
                else if(item.itemType == ArmorItemType.Chest)
                {
                    ArmorSlots[1].AddItem(item);
                    Debug.Log("it a armor");
                }
                else if(item.itemType == ArmorItemType.Leg)
                {
                    ArmorSlots[2].AddItem(item);
                    Debug.Log("it a leg");
                }
                else if(item.itemType == ArmorItemType.Foot)
                {
                    ArmorSlots[3].AddItem(item);
                    Debug.Log("it a foot");
                }
                else
                {
                    Debug.Log("unknow issue");
                }
            }
            else
            {
                Debug.Log("it full");//ArmorSlots[i].ClearSlot();
            }
        }
    }

    private void SetUpArmorSlots()
    {
        for(int i = 0; i < ArmorSlotSize; i++)
        {
            ItemSlot slot = gameObject.transform.GetChild(i).GetComponent<ItemSlot>();
            ArmorSlots.Add(slot);
        }
    }

}
