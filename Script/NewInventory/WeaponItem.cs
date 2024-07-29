using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/WeaponItem")]
public class WeaponItem : Item
{

    public WeaponItemType itemType;
    public int amount;
    public GameObject weapon;

    public override void Use()
    {
        base.Use();
        GameManager.instance.SwapWeapon(weapon);
    }
}


public enum WeaponItemType
{
    Melee,
    Ranged,
    Magic
}