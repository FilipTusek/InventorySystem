using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{   
    public EquipmentSlotType EquipmentSlot;

    public int StrengthModifier;
    public int DexterityModifier;
    public int AgilityModifier;
    public int InteligenceModifier;

    public int Durability;

    private void Awake ( )
    {
        TypeOfItem = ItemType.EquipableItem;
    }

    public override void Use ( )
    {
        base.Use ();       
    }
}

public enum EquipmentSlotType { Head, MainHand, OffHand, Feet }