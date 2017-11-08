using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour, IPointerClickHandler
{
    public Item MyItem;

    public float PickupRadius = 1.0f;

    public float DistanceToPlayer;

    private Transform _player;
    private PlayerController _playerController;
    private Inventory _inventory;
    private EquipmentManager _equipmentManager;

    private void Awake ( )
    {
        _player = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    private void Start ( )
    {
        _playerController = PlayerController.instance;
        _inventory = Inventory.instance;
        _equipmentManager = EquipmentManager.instance;
    }

    private void Update ( )
    {
        DistanceToPlayer = Vector2.Distance(transform.position, _player.position);        
    }

    public void PickUp()
    {
        switch(MyItem.TypeOfItem)
        {
            case ItemType.EquipableItem:
                Equipment equipment = (Equipment) MyItem;

                int slotIndex = (int) equipment.EquipmentSlot;

                if (_equipmentManager.CurrentEquipment [slotIndex] == null)
                {
                    _equipmentManager.Equip (equipment);
                    Destroy (gameObject);
                }
                else
                {
                    _inventory.Add (MyItem);
                    Destroy (gameObject);
                }
                break;

            case ItemType.StackableItem:
                _inventory.Add (MyItem);
                Destroy (gameObject);
                break;

            case ItemType.PermanentUsageItem:
                break;
        }             
    }

    private void OnDrawGizmosSelected ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, PickupRadius);
    }  
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_playerController.PickUpItemOnCollision)
        {
            if (DistanceToPlayer <= PickupRadius)
            {
                PickUp ();
            }
        }
    }
}
