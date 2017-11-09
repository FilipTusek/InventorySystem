using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public static DragAndDropManager instance;

    public bool ItemBeingDragged = false;
    public bool DropItem = false;

    public InventorySlot DraggedItemSlot;
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
        if(Input.GetMouseButtonUp(0))
        {
            if(DropItem && DraggedItemSlot.Item != null)
            {
                Instantiate (DraggedItemSlot.Item.ItemPrefab, _playerTransform.position + Vector3.right, Quaternion.identity);
                DraggedItemSlot.Item.RemoveFromInventroy ();
                ItemBeingDragged = false;
                DraggedItem.IsDragged = false;
                DraggedItem = null;
                DraggedItemSlot = null;
            }            
        }
    }
}
