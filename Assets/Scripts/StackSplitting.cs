using UnityEngine;
using UnityEngine.UI;

public class StackSplitting : MonoBehaviour
{
    public static StackSplitting instance;

    public InputField StackText;

    public Slider StackSlider;

    public InventorySlot Slot;

    public InventorySlot [] Slots;

    private Inventory _inventory;
    private InventoryUI _inventoryUI;

    public int StackAmount;
    public int StackMaximum;    

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start ( )
    {
        StackSlider = GetComponentInChildren<Slider> ();
        _inventory = Inventory.instance;
        _inventoryUI = InventoryUI.instance;
    }

    public void IncrementStack()
    {
        if(StackAmount < StackMaximum - 1)
        {
            StackAmount++;
            StackText.text = StackAmount.ToString();
            StackSlider.value = StackAmount;
        }
    }

    public void DecrementStack()
    {
        if(StackAmount > 1)
        {
            StackAmount--;
            StackText.text = StackAmount.ToString ();
            StackSlider.value = StackAmount;
        }
    }

    public void UpdateSliderValue()
    {
        StackAmount = (int) StackSlider.value;
        StackText.text = StackAmount.ToString ();
    }

    public void UpdateInputValue()
    {        
        StackAmount = int.Parse(StackText.text);         
        StackSlider.value = StackAmount;
    }

    public void SplitStack()
    {
        Slot.StackableItemData.StackSize = StackAmount;
        Slot.StackableItemData.UpdateStack ();

        foreach (InventorySlot slot in Slots)
        {
            if(slot.Item != null && slot.Item.ItemName == Slot.Item.ItemName)
            {
                if (slot.StackableItemData != null)
                {
                    slot.StackableItemData.ItemSplit = true;
                }
            }
        }

        for (int i = 0; i < StackMaximum - (StackAmount - 1); i++)
        {
            _inventory.Add (Slot.Item);
        }

        foreach (InventorySlot slot in Slots)
        {
            if (slot.Item != null && slot.Item.ItemName == Slot.Item.ItemName)
            {
                if (slot.StackableItemData != null)
                {
                    slot.StackableItemData.ItemSplit = false;
                }
            }
        }

        gameObject.SetActive (false);
    }

    public void CancelSplitting()
    {        
        gameObject.SetActive (false);
    }
}
