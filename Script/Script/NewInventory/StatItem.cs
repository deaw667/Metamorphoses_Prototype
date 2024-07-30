using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "StatItem", menuName = "Item/StatItem")]
public class StatItem : Item
{

    public StatItemType itemType;
    public int amount;
    public UnityEvent OnStatItemUsed;

    public override void Use()
    {
        base.Use();
        //PlayerHealth.instance.HealingPlayer(amount);
        GameManager.instance.OnStatItemuUse(itemType, amount);
        Inventory.instance.RemoveItemAmount(this, 1);

        OnStatItemUsed?.Invoke();
    }
}


public enum StatItemType
{
    HealthItem,
    ThirstItem,
    FoodItem
}