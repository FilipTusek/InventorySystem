using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [Header ("Inventory Equipment Slots")]
    public Image HeadSlotImage;
    public Image MainHandSlotImage;
    public Image OffHandSlotImage;
    public Image FeetSlotImage;
    
    public delegate void OnEquipmentChanged ( Equipment newItem, Equipment oldItem );
    public OnEquipmentChanged OnEquipmentChangedCallback;

    public Equipment [] CurrentEquipment;

    private Inventory _inventory;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start ( )
    {
        _inventory = Inventory.instance;
        int numberOfSlots = System.Enum.GetNames (typeof (EquipmentSlotType)).Length;
        CurrentEquipment = new Equipment [numberOfSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int) newItem.EquipmentSlot;

        Equipment oldItem = null;

        if(CurrentEquipment[slotIndex] != null)
        {
            oldItem = CurrentEquipment [slotIndex];
            _inventory.Add (oldItem);
        }

        if(OnEquipmentChangedCallback != null)
        {
            OnEquipmentChangedCallback.Invoke (newItem, oldItem);
        }

        CurrentEquipment [slotIndex] = newItem;

        switch(newItem.EquipmentSlot)
        {
            case EquipmentSlotType.Head:
                HeadSlotImage.enabled = true;
                HeadSlotImage.sprite = newItem.Icon;
                break;

            case EquipmentSlotType.MainHand:
                MainHandSlotImage.enabled = true;
                MainHandSlotImage.sprite = newItem.Icon;
                break;

            case EquipmentSlotType.OffHand:
                OffHandSlotImage.enabled = true;
                OffHandSlotImage.sprite = newItem.Icon;
                break;

            case EquipmentSlotType.Feet:
                FeetSlotImage.enabled = true;
                FeetSlotImage.sprite = newItem.Icon;
                break;
        }
    }

    public void UnequipItem(int slotIndex)
    {
        if(CurrentEquipment[slotIndex] != null)
        {
            Equipment oldItem = CurrentEquipment [slotIndex];
            _inventory.Add (oldItem);

            CurrentEquipment [slotIndex] = null;

            if (OnEquipmentChangedCallback != null)
            {
                OnEquipmentChangedCallback.Invoke (null, oldItem);
            }
        }
    }
}
