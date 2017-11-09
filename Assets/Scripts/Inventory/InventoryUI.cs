using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform ItemsParent;

    public GameObject InventorySlotGameObject;

    private InventroySlot [] slots; 

    private Inventory _inventory;

    private bool _stacked = false;

    private void Start ( )
    {
        for (int i = 0; i < 32; i++)
        {
            Instantiate (InventorySlotGameObject, ItemsParent);
        }

        _inventory = Inventory.instance;
        _inventory.OnItemChangedCallback += UpdateUI;

        slots = ItemsParent.GetComponentsInChildren<InventroySlot> ();
    }

    private void UpdateUI()
    {
        if (_inventory.Items.Count > slots.Length)
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate (InventorySlotGameObject, ItemsParent);
                slots = ItemsParent.GetComponentsInChildren<InventroySlot> ();
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if(i < _inventory.Items.Count)
            {
                if(_inventory.Items[_inventory.Items.Count - 1].TypeOfItem == ItemType.StackableItem)
                {
                    if (_inventory.Items [i].TypeOfItem == ItemType.StackableItem)
                    {
                        StackableItem stackableItem = (StackableItem) _inventory.Items [i];                        

                        if (slots [i].Item != null && slots [i].Item.ItemName == _inventory.Items [_inventory.Items.Count - 1].ItemName)
                        {
                            slots [i].StackableItemData.LimitedStackSize = stackableItem.HasStackLimit;

                            if (slots [i].StackableItemData.LimitedStackSize)
                            {
                                if (slots [i].StackableItemData.StackSize < slots [i].StackableItemData.StackLimit)
                                {
                                    if (!_stacked)
                                    {
                                        slots [i].StackableItemData.StackSize++;
                                        slots [i].StackableItemData.UpdateStack ();

                                        _inventory.Items.RemoveAt (_inventory.Items.Count - 1);
                                        _stacked = true;
                                    }
                                }
                                else
                                {
                                    slots [i].AddItem (_inventory.Items [i]);
                                }
                            }
                            else
                            {
                                if (!_stacked)
                                {
                                    slots [i].StackableItemData.StackSize++;
                                    slots [i].StackableItemData.UpdateStack ();

                                    _inventory.Items.RemoveAt (_inventory.Items.Count - 1);
                                    _stacked = true;
                                }
                            }
                        }
                        else
                        {
                            slots [i].AddItem (_inventory.Items [i]);
                        }
                    }
                }
                else
                {
                    slots [i].AddItem (_inventory.Items [i]);
                }                
            }
            else
            {
                slots [i].ClearSlot ();
            }
        }
        _stacked = false;
    }    
}
