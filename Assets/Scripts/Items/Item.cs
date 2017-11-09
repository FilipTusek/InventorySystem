using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string ItemName = "New Item";

    public Sprite Icon = null;

    public GameObject ItemPrefab;
        
    public ItemType TypeOfItem;    

    public virtual void Use()
    {

    }

    public void RemoveFromInventroy()
    {
        Inventory.instance.Remove (this);
    }
}

public enum ItemType { PermanentUsageItem, EquipableItem, StackableItem }
