using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public Text StackNumber;

    public StackableItemData StackableItemData;

    public Item Item;

    public bool IsEmpty = true;

    public void AddItem(Item newItem)
    {
        Item = newItem;

        Icon.sprite = Item.Icon;
        Icon.enabled = true;

        if(newItem.TypeOfItem == ItemType.StackableItem)
        {
            StackableItem stackableItem = (StackableItem) newItem;

            StackableItemData = GetComponent<StackableItemData> ();

            if (stackableItem.HasStackLimit)
            {
                StackNumber.enabled = true;
                StackableItemData.StackLimit = stackableItem.StackLimit;
                StackableItemData.LimitedStackSize = stackableItem.HasStackLimit;
                StackableItemData.UpdateStack ();
            }
            else
            {
                StackNumber.enabled = true;
                StackableItemData.LimitedStackSize = stackableItem.HasStackLimit;
                StackableItemData.UpdateStack ();
            }
        }
        else
        {
            StackNumber.enabled = false;
        }

        IsEmpty = false;
    } 
    
    public void ClearSlot()
    {
        Item = null;

        Icon.sprite = null;
        Icon.enabled = false;        
        StackNumber.enabled = false;
        IsEmpty = true;

        if(StackableItemData != null)
        {
            StackableItemData.StackSize = 1;
        }
    }  

    public void UseItem()
    {
        if(Item != null)
        {
            Item.Use ();
        }
    }
}
