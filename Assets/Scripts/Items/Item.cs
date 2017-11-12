using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string ItemName = "New Item";

    public Sprite Icon = null;

    public GameObject ItemPrefab;

    public enum CategoryType { Weapon, Shield, Helmet, Boots, Potion, Misc }
    public CategoryType ItemCategory;
        
    public ItemType TypeOfItem;    

    public virtual void Use()
    {
        Analytics.CustomEvent ("Item USed", new Dictionary<string, object>
        {
            {"Item Name", ItemName},
            {"Item Type", TypeOfItem.ToString()}
        });
    }

    public void RemoveFromInventroy()
    {
        Inventory.instance.Remove (this);
    }
}

public enum ItemType { PermanentUsageItem, EquipableItem, StackableItem }
