using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform ItemsParent;

    public GameObject InventorySlotGameObject;

    private InventroySlot [] slots; 

    private Inventory _inventory;

    private void Start ( )
    {
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
                slots [i].AddItem (_inventory.Items [i]);
            }
            else
            {
                slots [i].ClearSlot ();
            }
        }       
    }    
}
