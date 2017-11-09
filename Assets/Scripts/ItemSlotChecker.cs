using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public EquipmentSlotType SlotEquipmentSlot;

    public Image SlotImage;    

    private DragAndDropManager _dragAndDropManager;    

    private void Start ( )
    {
        _dragAndDropManager = DragAndDropManager.instance;
        SlotImage = GetComponentInChildren<Image> ();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if(_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItemSlot.Item.TypeOfItem == ItemType.EquipableItem)
        {
            Equipment item = (Equipment) _dragAndDropManager.DraggedItemSlot.Item;

            if(item.EquipmentSlot == SlotEquipmentSlot)
            {
                if (_dragAndDropManager.DraggedItem != null)
                {
                    _dragAndDropManager.DraggedItem.OverAvailableSlot = true;
                }
            }
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItemSlot.Item.TypeOfItem == ItemType.EquipableItem)
        {
            Equipment item = (Equipment) _dragAndDropManager.DraggedItemSlot.Item;

            if (item.EquipmentSlot == SlotEquipmentSlot)
            {
                if (_dragAndDropManager.DraggedItem != null)
                {
                    _dragAndDropManager.DraggedItem.OverAvailableSlot = false;
                }
            }
        }
    }

    public void OnPointerClick ( PointerEventData eventData )
    {
        if (_dragAndDropManager.DraggedItemSlot != null)
        {
            _dragAndDropManager.DraggedItem.DragOrDrop ();
        }
    }
}
