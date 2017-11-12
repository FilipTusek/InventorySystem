using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;

public class ItemPickup : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item MyItem;

    public float PickupRadius = 1.0f;

    public float DistanceToPlayer;

    public Color HighlitedColor;

     private SpriteRenderer _spriteRenderer;

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
        _spriteRenderer = GetComponent<SpriteRenderer> ();
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
                MyItem.Use ();
                Destroy (gameObject);
                break;
        }

        Analytics.CustomEvent ("Item Pickup", new Dictionary<string, object>
        {
            {"Item Name", MyItem.ItemName},
            {"Item Type", MyItem.TypeOfItem.ToString()}
        });
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _spriteRenderer.color = HighlitedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.white;
    }
}
