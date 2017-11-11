using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool OverAvailableSlot = false;
    public bool OverInventorySlot = false;
    public bool IsDragged = false;

    public InventorySlot Slot;    

    private Transform _slotTransform;

    private GameObject _inventoryScreen;
    private GameObject _equipmentScreen;
    private GameObject _equipmentTooltipScreen;
    private GameObject _consumableTooltipScreen;

    private DragAndDropManager _dragAndDropManager;
    private EquipmentManager _equipmentManager;
    private Inventory _inventory;
    private InventoryUI _inventoryUI;
    private Tooltip _equipmentTooltip;
    private Tooltip _consumableTooltip;

    private Image _itemImage;

    private bool _pointerOver = false;  

    private void Start ( )
    {
        _dragAndDropManager = DragAndDropManager.instance;
        _equipmentManager = EquipmentManager.instance;
        _inventory = Inventory.instance;
        _inventoryUI = InventoryUI.instance;
        
        Slot = GetComponentInParent<InventorySlot> ();       
        _itemImage = GetComponent<Image> ();

        _slotTransform = transform.parent.transform;

        _inventoryScreen = _inventoryUI.Panel.InventoryScreen;
        _equipmentScreen = _inventoryUI.Panel.EquipmentScreen;
        _equipmentTooltipScreen = _inventoryUI.Panel.EquipmentTooltipScreen;
        _consumableTooltipScreen = _inventoryUI.Panel.ConsumableTooltipScreen;

        _equipmentTooltip = _equipmentTooltipScreen.GetComponent<Tooltip> ();
        _consumableTooltip = _consumableTooltipScreen.GetComponent<Tooltip> ();
    }

    private void Update ( )
    {
        SetItemPosition ();
        CheckForItemDrop ();
        CheckForEquip ();
    }

    private void SetItemPosition ( )
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

    private void CheckForItemDrop ( )
    {
        if (Input.GetMouseButton (0) && Input.GetKeyDown (KeyCode.T) && _pointerOver)
        {
            _dragAndDropManager.DraggedItemSlot = _slotTransform.gameObject.GetComponent<InventorySlot> ();
            _dragAndDropManager.DraggedItem = this;
            _dragAndDropManager.DropItem ();
        }

        if (_pointerOver && Input.GetKeyDown (KeyCode.Z))
        {
            _dragAndDropManager.DraggedItemSlot = _slotTransform.gameObject.GetComponent<InventorySlot> ();
            _dragAndDropManager.DraggedItem = this;
            _dragAndDropManager.DropItem ();
        }
    }

    private void CheckForEquip ( )
    {
        if (_pointerOver && Input.GetKeyDown (KeyCode.Q))
        {
            EquipItem ();
        }
    }

    private void ShowTooltip ( )
    {
        if (_dragAndDropManager.DraggedItem == null)
        {
            if (Slot.Item.TypeOfItem == ItemType.EquipableItem)
            {
                _equipmentTooltip.ItemName.text = Slot.Item.ItemName;
                _equipmentTooltip.TypeOfItem.text = Slot.Item.ItemCategory.ToString ();
                _equipmentTooltip.ItemIcon.sprite = Slot.Item.Icon;

                Equipment equipment = (Equipment) Slot.Item;

                _equipmentTooltip.EquipTooltip.Strength.text = "Strength: " + equipment.StrengthModifier.ToString ();
                _equipmentTooltip.EquipTooltip.Dexterity.text = "Dexterity: " + equipment.DexterityModifier.ToString ();
                _equipmentTooltip.EquipTooltip.Agility.text = "Agility: " + equipment.AgilityModifier.ToString ();
                _equipmentTooltip.EquipTooltip.Inteligence.text = "Inteligence: " + equipment.InteligenceModifier.ToString ();

                _equipmentTooltipScreen.SetActive (true);
            }
            else if (Slot.Item.TypeOfItem == ItemType.StackableItem)
            {
                _consumableTooltip.ItemName.text = Slot.Item.ItemName;
                _consumableTooltip.TypeOfItem.text = Slot.Item.ItemCategory.ToString ();
                _consumableTooltip.ItemIcon.sprite = Slot.Item.Icon;

                StackableItem consumable = (StackableItem) Slot.Item;

                _consumableTooltip.ConsumTooltip.Description.text = consumable.ItemDescription;
                _consumableTooltipScreen.SetActive (true);
            }
        }
    }

    private void HideTooltip ( )
    {
        _equipmentTooltipScreen.SetActive (false);
        _consumableTooltipScreen.SetActive (false);
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

    private void UseItem()
    {
        Slot.Item.Use ();
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

        if(eventData.button == PointerEventData.InputButton.Middle)
        {
            if (Slot.Item.ItemCategory == Item.CategoryType.Potion)
            {
                UsableItem usable = (UsableItem) Slot.Item;

                usable.Use ();

                if(Slot.StackableItemData.StackSize > 1)
                {
                    Slot.StackableItemData.StackSize--;
                    Slot.StackableItemData.UpdateStack ();
                }
                else
                {
                    Slot.Item.RemoveFromInventroy ();
                    Slot.ClearSlot ();
                }
            }
        }
    } 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOver = true;
        ShowTooltip ();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOver = false;
        HideTooltip ();
    }
}
