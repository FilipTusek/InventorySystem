using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public Text StackNumber;

    public StackableItemData StackableItemData;

    public Item Item;     

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
                StackableItemData.UpdateStack ();
            }
            else
            {
                StackNumber.enabled = true;
                StackableItemData.UpdateStack ();
            }
        }
        else
        {
            StackNumber.enabled = false;
        }
    } 
    
    public void ClearSlot()
    {
        Item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }

    public void DropItem()
    {
        Inventory.instance.Remove (Item);
        //Drop item to the floor
    }

    public void UseItem()
    {
        if(Item != null)
        {
            Item.Use ();
        }
    }
}
