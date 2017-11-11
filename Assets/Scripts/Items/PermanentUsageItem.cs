using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Inventory/Item/PermanentUsageItem")]
public class PermanentUsageItem : Item
{
    public override void Use ( )
    {
        base.Use ();

        Debug.Log (ItemName + " consumed!");
    }
}
