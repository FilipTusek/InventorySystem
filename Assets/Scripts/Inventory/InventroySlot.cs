using UnityEngine;
using UnityEngine.UI;

public class InventroySlot : MonoBehaviour
{
    public Image Icon;

    private Item _item;

    public void AddItem(Item newItem)
    {
        _item = newItem;

        Icon.sprite = _item.Icon;
        Icon.enabled = true;
    } 
    
    public void ClearSlot()
    {
        _item = null;

        Icon.sprite = null;
        Icon.enabled = false;
    }

    public void DropItem()
    {
        Inventory.instance.Remove (_item);
        //Drop item to the floor
    }

    public void UseItem()
    {
        if(_item != null)
        {
            _item.Use ();
        }
    }
}
