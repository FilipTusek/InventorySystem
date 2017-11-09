using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerClickHandler
{
    public bool OverAvailableSlot = false;

    public InventorySlot Slot;

    private Transform _playerTransform;
    private Transform _slotTransform;

    private GameObject _inventoryScreen;
    private GameObject _equipmentScreen;    

    private bool _isDragged = false;

    private DragAndDropManager _dragAndDropManager;
    private EquipmentManager _equipmentManager;
    

    private void Awake ( )
    {
        _playerTransform = GameObject.Find ("Player").transform;
        _inventoryScreen = GameObject.Find ("InventoryPanel");
        _equipmentScreen = GameObject.Find ("EquipmentPanel");
    }

    private void Start ( )
    {
        _dragAndDropManager = DragAndDropManager.instance;
        _slotTransform = transform.parent.parent.transform;
        Slot = GetComponentInParent<InventorySlot>();
        _equipmentManager = EquipmentManager.instance;
    }

    private void Update ( )
    {
        if(_isDragged)
        {
            transform.position = Input.mousePosition;            
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }
    }

    public void DragOrDrop()
    {
        if (!_dragAndDropManager.ItemBeingDragged)
        {
            if (!_isDragged)
            {
                _isDragged = true;
                _dragAndDropManager.ItemBeingDragged = true;
                transform.SetParent (transform.root.transform);
                _dragAndDropManager.DraggedItemSlot = _slotTransform.gameObject.GetComponent<InventorySlot> ();
                _dragAndDropManager.DraggedItem = this;
                gameObject.GetComponent<Image> ().raycastTarget = false;                
            }            
        }
        else
        {
            if (_isDragged)
            {
                if (!OverAvailableSlot)
                {
                    _isDragged = false;
                    _dragAndDropManager.ItemBeingDragged = false;
                    transform.SetParent (_slotTransform);
                    _dragAndDropManager.DraggedItem = null;
                    gameObject.GetComponent<Image> ().raycastTarget = true;
                }
                else
                {
                    _isDragged = false;
                    _dragAndDropManager.ItemBeingDragged = false;                    
                    _equipmentManager.Equip ((Equipment) Slot.Item);
                    Slot.Item.RemoveFromInventroy ();
                    transform.SetParent (_slotTransform);
                    _dragAndDropManager.DraggedItem = null;
                    gameObject.GetComponent<Image> ().raycastTarget = true;
                }
            }
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        DragOrDrop ();
    }
}
