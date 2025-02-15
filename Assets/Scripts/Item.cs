using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite image;
    public ItemType itemType;
    public ActionType actionType;
    public bool stackable = true;
    
    // weapon
    public bool isWeapon;
    public int damage;
    
    // armor
    public bool isArmor;
    public int defense;
    
    // health container
    public bool isHealthContainer;
    public int healthAmount;
    
    // energy container
    public bool isEnergyContainer;
    public int energyAmount;
}

public enum ItemType
{
    Weapon,
    Armor,
    HealthContainer,
    EnergyContainer,
}

public enum ActionType
{
    None,
    Equip,
    Consume,
}
