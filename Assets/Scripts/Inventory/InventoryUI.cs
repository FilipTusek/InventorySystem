using UnityEngine;
using System;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Transform ItemsParent;

    public GameObject InventorySlotGameObject;

    [Serializable]
    public struct InventoryPanel
    {
        public GameObject InventoryScreen;
        public GameObject EquipmentScreen;
        public GameObject AttributesScreen;

        public GameObject EquipmentTooltipScreen;
        public GameObject ConsumableTooltipScreen;

        public GameObject InventroyToggleButton;
        public GameObject EquipmentToggleButton;
        public GameObject AttributesToggleButton;
    }

    public InventoryPanel Panel;

    private InventorySlot [] slots; 

    private Inventory _inventory;

    private DragAndDropManager _dragAndDropManager;

    private bool _stacked = false;
    private bool _itemAdded = false;

    private int _inventoryLength = 0;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start ( )
    {
        for (int i = 0; i < 32; i++)
        {
            Instantiate (InventorySlotGameObject, ItemsParent);
        }

        _inventory = Inventory.instance;
        _inventory.OnItemChangedCallback += UpdateUI;

        _dragAndDropManager = DragAndDropManager.instance;

        slots = ItemsParent.GetComponentsInChildren<InventorySlot> ();

        DisableInventoryPanels ();
    }

    public void DisableInventoryPanels ( )
    {
        Panel.InventoryScreen.SetActive (false);
        Panel.EquipmentScreen.SetActive (false);
        Panel.AttributesScreen.SetActive (false);

        Panel.EquipmentTooltipScreen.SetActive (false);
        Panel.ConsumableTooltipScreen.SetActive (false);        
    }

    private void UpdateUI()
    {
        AddRowToInventory ();
        RemoveRowFromInventory ();

        _itemAdded = false;

        //Handles item icons and stacks inside inventory UI
        if (_inventoryLength <= _inventory.Items.Count)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Item item = _inventory.Items [_inventory.Items.Count - 1];

                if (item.TypeOfItem == ItemType.EquipableItem)
                {
                    if (slots [i].IsEmpty && !_itemAdded)
                    {
                        slots [i].AddItem (item);
                        _itemAdded = true;
                    }
                }
                else if (item.TypeOfItem == ItemType.StackableItem)
                {
                    if (!_itemAdded)
                    {                        
                        if (slots [i].Item != null)  
                        {
                            if (slots [i].Item.ItemName == item.ItemName)
                            {
                                if (slots [i].StackableItemData.LimitedStackSize && slots[i].StackableItemData.StackSize < slots [i].StackableItemData.StackLimit)
                                {
                                    if (!_stacked)
                                    {
                                        slots [i].StackableItemData.StackSize++;
                                        slots [i].StackableItemData.UpdateStack ();
                                        _inventory.Items.Remove (item);
                                        RemoveRowFromInventory ();
                                        _itemAdded = true;
                                        _stacked = true;
                                    }
                                }
                                else if(!slots [i].StackableItemData.LimitedStackSize)
                                {
                                    if(!_stacked)
                                    {
                                        slots [i].StackableItemData.StackSize++;
                                        slots [i].StackableItemData.UpdateStack ();
                                        _inventory.Items.Remove (item);
                                        RemoveRowFromInventory ();
                                        _itemAdded = true;
                                        _stacked = true;
                                    }
                                }                                
                            }                            
                        }
                        else
                        {
                            if (!_itemAdded)
                            {
                                slots [i].AddItem (item);                               
                                _itemAdded = true;
                            }
                        }
                    }
                }
            }
        }       

        _inventoryLength = _inventory.Items.Count;
        _stacked = false;        

        //for (int i = 0; i < slots.Length; i++)
        //{
        //    if(i < _inventory.Items.Count)
        //    {
        //        if(_inventory.Items[_inventory.Items.Count - 1].TypeOfItem == ItemType.StackableItem)
        //        {
        //            if (_inventory.Items [i].TypeOfItem == ItemType.StackableItem)
        //            {
        //                StackableItem stackableItem = (StackableItem) _inventory.Items [i];                        

        //                if (slots [i].Item != null && slots [i].Item.ItemName == _inventory.Items [_inventory.Items.Count - 1].ItemName)
        //                {
        //                    slots [i].StackableItemData.LimitedStackSize = stackableItem.HasStackLimit;

        //                    if (slots [i].StackableItemData.LimitedStackSize)
        //                    {
        //                        if (slots [i].StackableItemData.StackSize < slots [i].StackableItemData.StackLimit)
        //                        {
        //                            if (!_stacked)
        //                            {
        //                                slots [i].StackableItemData.StackSize++;
        //                                slots [i].StackableItemData.UpdateStack ();

        //                                _inventory.Items.RemoveAt (_inventory.Items.Count - 1);
        //                                _stacked = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            slots [i].AddItem (_inventory.Items [i]);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (!_stacked)
        //                        {
        //                            slots [i].StackableItemData.StackSize++;
        //                            slots [i].StackableItemData.UpdateStack ();

        //                            _inventory.Items.RemoveAt (_inventory.Items.Count - 1);
        //                            _stacked = true;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    slots [i].AddItem (_inventory.Items [i]);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            slots [i].AddItem (_inventory.Items [i]);
        //        }                
        //    }
        //    else
        //    {
        //        slots [i].ClearSlot ();
        //    }
        //}
        //_stacked = false;         
    }  
    
    private void AddRowToInventory()
    {        
        if (_inventory.Items.Count > slots.Length)
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate (InventorySlotGameObject, ItemsParent);
                slots = ItemsParent.GetComponentsInChildren<InventorySlot> ();
            }
        }        
    }

    private void RemoveRowFromInventory()
    {        
        if (_inventory.Items.Count >= 32)
        {
            if (_inventory.Items.Count <= slots.Length - 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    Destroy (slots [slots.Length - (i + 1)].gameObject);
                }
                System.Array.Resize (ref slots, slots.Length - 8);
            }
        }
    }
}
