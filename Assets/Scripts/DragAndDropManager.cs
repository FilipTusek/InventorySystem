using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public static DragAndDropManager instance;

    public bool TouchInputEnabled = false;

    public bool ItemBeingDragged = false;
    public bool Drop = false;

    public InventorySlot DraggedItemSlot;
    public InventorySlot NewSlot;
    public DragAndDrop DraggedItem;

    private Transform _playerTransform;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }

        _playerTransform = GameObject.Find ("Player").transform;
    }

    private void Update ( )
    {       
        if(Input.GetMouseButtonUp (0) && DraggedItemSlot != null && DraggedItemSlot.Item != null && Drop)
        {
            DropItem ();
        }
    }

    public void DropItem()
    {
        if (DraggedItem.Slot.Item.TypeOfItem == ItemType.EquipableItem)
        {
            Instantiate (DraggedItemSlot.Item.ItemPrefab, _playerTransform.position + Vector3.right, Quaternion.identity);

            DraggedItemSlot.Item.RemoveFromInventroy ();
            DraggedItemSlot.ClearSlot ();

            ItemBeingDragged = false;
            DraggedItem.IsDragged = false;

            DraggedItem = null;
            DraggedItemSlot = null;
        }
        else if (DraggedItem.Slot.Item.TypeOfItem == ItemType.StackableItem)
        {
            for (int i = 0; i < DraggedItem.Slot.StackableItemData.StackSize; i++)
            {
                Instantiate (DraggedItemSlot.Item.ItemPrefab, _playerTransform.position + Vector3.right, Quaternion.identity);
            }

            DraggedItemSlot.Item.RemoveFromInventroy ();
            DraggedItemSlot.ClearSlot ();

            ItemBeingDragged = false;
            DraggedItem.IsDragged = false;

            DraggedItem = null;
            DraggedItemSlot = null;
        }
    }
}
