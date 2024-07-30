using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/ArmorItem")]
public class ArmorItem : Item
{

    public ArmorItemType itemType;
    public int amount;

    public override void Use()
    {

    }
}


public enum ArmorItemType
{
    Head,
    Chest,
    Leg,
    Foot
}