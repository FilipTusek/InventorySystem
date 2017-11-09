using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerClickHandler
{
    public bool OverAvailableSlot = false;
    public bool OverInventorySlot = false;
    public bool IsDragged = false;

    public InventorySlot Slot;
   
    private Transform _slotTransform;

    private GameObject _inventoryScreen;
    private GameObject _equipmentScreen;      

    private DragAndDropManager _dragAndDropManager;
    private EquipmentManager _equipmentManager;
    private Inventory _inventory;

    private Image _itemImage;

    private void Awake ( )
    {
        _inventoryScreen = GameObject.Find ("InventoryPanel");
        _equipmentScreen = GameObject.Find ("EquipmentPanel");
    }

    private void Start ( )
    {       
        _dragAndDropManager = DragAndDropManager.instance;
        _slotTransform = transform.parent.transform;
        Slot = GetComponentInParent<InventorySlot>();
        _equipmentManager = EquipmentManager.instance;
        _inventory = Inventory.instance;
        _itemImage = GetComponent<Image> ();
    }

    private void Update ( )
    {
        if (IsDragged)
        {
            if (_inventoryScreen.activeSelf || _equipmentScreen.activeSelf)
            {
                transform.position = Input.mousePosition;
            }
            else
            {
                IsDragged = false;
                _dragAndDropManager.ItemBeingDragged = false;
                transform.SetParent (_slotTransform);
                _dragAndDropManager.DraggedItem = null;
                _itemImage.raycastTarget = true;
            }
        }
        else
        {
            transform.SetParent (_slotTransform);
            transform.localPosition = Vector2.zero;
            _itemImage.raycastTarget = true;
        }
    }

    public void DragOrDrop()
    {
        if (!_dragAndDropManager.ItemBeingDragged)
        {
            if (!IsDragged)
            {
                IsDragged = true;
                _dragAndDropManager.ItemBeingDragged = true;
                transform.SetParent (transform.root.transform);
                _dragAndDropManager.DraggedItemSlot = _slotTransform.gameObject.GetComponent<InventorySlot> ();
                _dragAndDropManager.DraggedItem = this;
                _itemImage.raycastTarget = false;                
            }            
        }
        else
        {
            if (IsDragged)
            {
                if (!OverAvailableSlot)
                {
                    if (!OverInventorySlot)
                    {
                        IsDragged = false;
                        _dragAndDropManager.ItemBeingDragged = false;
                        transform.SetParent (_slotTransform);
                        _dragAndDropManager.DraggedItem = null;
                        _itemImage.raycastTarget = true;
                    }
                    else
                    {
                        IsDragged = false;
                        _dragAndDropManager.ItemBeingDragged = false;
                        transform.SetParent (_slotTransform);
                        _dragAndDropManager.DraggedItem = null;
                        _itemImage.raycastTarget = true;
                        _inventory.Add (Slot.Item);
                        Slot.Item.RemoveFromInventroy ();                        
                    }
                }
                else if (OverAvailableSlot)
                {
                    IsDragged = false;
                    _dragAndDropManager.ItemBeingDragged = false;                    
                    _equipmentManager.Equip ((Equipment) Slot.Item);
                    Slot.Item.RemoveFromInventroy ();
                    transform.SetParent (_slotTransform);
                    _dragAndDropManager.DraggedItem = null;
                    _itemImage.raycastTarget = true;
                }                            
            }
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        DragOrDrop ();
    }
}
