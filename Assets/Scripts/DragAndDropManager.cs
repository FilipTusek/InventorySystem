using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public static DragAndDropManager instance;

    public bool ItemBeingDragged = false;

    public InventorySlot DraggedItemSlot;
    public DragAndDrop DraggedItem;    

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
