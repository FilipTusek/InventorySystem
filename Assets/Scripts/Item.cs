using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string ItemName = "New Item";

    public Sprite Icon = null;    
    
    public enum ItemType { PermanentUsageItem, EquipableItem, StackableItem }
    public ItemType TypeOfItem;

    public bool HasStackLimit = true;
}
