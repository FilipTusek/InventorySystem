﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum SlotType { EquipmentSlot, InventorySlot }
    public SlotType TypeOfSlot;

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
        switch (TypeOfSlot)
        {
            case SlotType.EquipmentSlot:
                if (_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItemSlot.Item.TypeOfItem == ItemType.EquipableItem)
                {
                    Equipment item = (Equipment) _dragAndDropManager.DraggedItemSlot.Item;

                    if (item.EquipmentSlot == SlotEquipmentSlot && _dragAndDropManager.DraggedItem != null)
                    {
                        _dragAndDropManager.DraggedItem.OverAvailableSlot = true;
                    }
                }
                break;

            case SlotType.InventorySlot:
                if (_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItem != null)
                {
                    _dragAndDropManager.DraggedItem.OverInventorySlot = true;
                }
                break;
        }       
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItemSlot.Item.TypeOfItem == ItemType.EquipableItem)
        {
            Equipment item = (Equipment) _dragAndDropManager.DraggedItemSlot.Item;

            if (item.EquipmentSlot == SlotEquipmentSlot && _dragAndDropManager.DraggedItem != null)
            {                
                _dragAndDropManager.DraggedItem.OverAvailableSlot = false;
                _dragAndDropManager.DraggedItem.OverInventorySlot = false;
            }
        }
    }

    public void OnPointerClick ( PointerEventData eventData )
    {
        if (_dragAndDropManager.DraggedItemSlot != null && _dragAndDropManager.DraggedItem != null)
        {            
            _dragAndDropManager.DraggedItem.DragOrDrop ();           
        }
    }
}