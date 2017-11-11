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

    private bool _pointerOver = false;

    private void Awake ( )
    {
        _equipmentScreen = GameObject.Find ("EquipmentPanel");
        _inventoryScreen = GameObject.Find ("InventoryPanel");        
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
                Slot.IsEmpty = true;
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
                        
                        _itemImage.raycastTarget = true;

                        _dragAndDropManager.DraggedItem = null;
                    }
                    else
                    {
                        IsDragged = false;
                        _dragAndDropManager.ItemBeingDragged = false;
                        transform.SetParent (_slotTransform);
                        
                        _itemImage.raycastTarget = true;

                        _dragAndDropManager.NewSlot.AddItem (_dragAndDropManager.DraggedItemSlot.Item);

                        if (_dragAndDropManager.NewSlot.StackableItemData != null && Slot.StackableItemData != null)
                        {
                            _dragAndDropManager.NewSlot.StackableItemData.StackSize = Slot.StackableItemData.StackSize;
                            _dragAndDropManager.NewSlot.StackableItemData.UpdateStack ();
                        }

                        if (_dragAndDropManager.NewSlot.GetInstanceID() != Slot.GetInstanceID())
                        {
                            Slot.ClearSlot ();
                        }

                        _dragAndDropManager.DraggedItem = null;
                        _dragAndDropManager.DraggedItemSlot = null;
                    }
                }
                else if (OverAvailableSlot)
                {
                    IsDragged = false;
                    _dragAndDropManager.ItemBeingDragged = false;   
                    
                    _equipmentManager.Equip ((Equipment) Slot.Item);
                    transform.SetParent (_slotTransform);

                    Slot.Item.RemoveFromInventroy ();
                    Slot.ClearSlot ();                    

                    _itemImage.raycastTarget = true;

                    _dragAndDropManager.DraggedItem = null;
                    _dragAndDropManager.DraggedItemSlot = null;
                }                            
            }
        }
    }

    public void EquipItem()
    {
        if (Slot.Item != null)
        {
            Equipment item = (Equipment) Slot.Item;
            Slot.ClearSlot ();
            _equipmentManager.Equip (item);
            item.RemoveFromInventroy ();
        }               
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DragOrDrop ();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Slot.Item.TypeOfItem == ItemType.EquipableItem)
            {
                EquipItem ();
            }
        }
    }   
}
