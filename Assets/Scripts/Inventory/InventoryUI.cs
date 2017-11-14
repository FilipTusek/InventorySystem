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
        public GameObject StackSplitScreen;

        public GameObject EquipmentTooltipScreen;
        public GameObject ConsumableTooltipScreen;

        public GameObject InventroyToggleButton;
        public GameObject EquipmentToggleButton;
        public GameObject AttributesToggleButton;
    }

    public InventoryPanel Panel;    

    public InventorySlot [] Slots; 

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

        Slots = ItemsParent.GetComponentsInChildren<InventorySlot> ();

        DisableInventoryPanels ();
    }

    public void DisableInventoryPanels ( )
    {
        Panel.InventoryScreen.SetActive (false);
        Panel.EquipmentScreen.SetActive (false);
        Panel.AttributesScreen.SetActive (false);
        Panel.StackSplitScreen.SetActive (false);

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
            for (int i = 0; i < Slots.Length; i++)
            {
                Item item = _inventory.Items [_inventory.Items.Count - 1];

                if (item.TypeOfItem == ItemType.EquipableItem)
                {
                    if (Slots [i].IsEmpty && !_itemAdded)
                    {
                        Slots [i].AddItem (item);
                        _itemAdded = true;
                    }
                }
                else if (item.TypeOfItem == ItemType.StackableItem)
                {
                    if (!_itemAdded)
                    {                        
                        if (Slots [i].Item != null)  
                        {
                            if (Slots [i].Item.ItemName == item.ItemName)
                            {
                                if (Slots [i].StackableItemData.LimitedStackSize && Slots[i].StackableItemData.StackSize < Slots [i].StackableItemData.StackLimit)
                                {
                                    if (!_stacked)
                                    {
                                        if (!Slots [i].StackableItemData.ItemSplit)
                                        {
                                            Slots [i].StackableItemData.StackSize++;
                                            Slots [i].StackableItemData.UpdateStack ();
                                            _inventory.Items.Remove (item);
                                            RemoveRowFromInventory ();
                                            _itemAdded = true;
                                            _stacked = true;
                                        }
                                    }
                                }
                                else if(!Slots [i].StackableItemData.LimitedStackSize)
                                {
                                    if(!_stacked)
                                    {
                                        if (!Slots [i].StackableItemData.ItemSplit)
                                        {
                                            Slots [i].StackableItemData.StackSize++;
                                            Slots [i].StackableItemData.UpdateStack ();
                                            _inventory.Items.Remove (item);
                                            RemoveRowFromInventory ();
                                            _itemAdded = true;
                                            _stacked = true;
                                        }
                                    }
                                }                                
                            }                            
                        }
                        else
                        {
                            if (!_itemAdded)
                            {
                                Slots [i].AddItem (item);                               
                                _itemAdded = true;
                            }
                        }
                    }
                }
            }
        }       

        _inventoryLength = _inventory.Items.Count;
        _stacked = false;              
    }  
    
    private void AddRowToInventory()
    {        
        if (_inventory.Items.Count > Slots.Length)
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate (InventorySlotGameObject, ItemsParent);
                Slots = ItemsParent.GetComponentsInChildren<InventorySlot> ();
            }
        }        
    }

    private void RemoveRowFromInventory()
    {        
        if (_inventory.Items.Count >= 32)
        {
            if (_inventory.Items.Count <= Slots.Length - 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    Destroy (Slots [Slots.Length - (i + 1)].gameObject);
                }
                System.Array.Resize (ref Slots, Slots.Length - 8);
            }
        }
    }
}
