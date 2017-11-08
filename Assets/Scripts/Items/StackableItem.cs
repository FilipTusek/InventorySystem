using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stackable Item", menuName = "Inventory/Stackable Item")]
public class StackableItem : Item
{
    public bool HasStackLimit = false;

    public int StackLimit = 5;

    private void Awake ( )
    {
        TypeOfItem = ItemType.StackableItem;
    }

    public override void Use ( )
    {
        base.Use ();
    }
}
