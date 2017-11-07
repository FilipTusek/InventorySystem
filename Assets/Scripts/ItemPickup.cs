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

    private void Awake ( )
    {
        _player = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    private void Start ( )
    {
        _playerController = PlayerController.instance;
        _inventory = Inventory.instance;
    }

    private void Update ( )
    {
        DistanceToPlayer = Vector2.Distance(transform.position, _player.position);        
    }

    public void PickUp()
    {
        _inventory.Add (MyItem);
        Destroy (gameObject);       
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
