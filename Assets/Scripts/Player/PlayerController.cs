using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool PickUpItemOnCollision = true;

    private InventoryUI _inventoryUI;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    private void Start ( )
    {
        _inventoryUI = InventoryUI.instance;
    }

    private void Update ( )
    {
        CheckForScreenToggle ();
    }

    private void CheckForScreenToggle()
    {
        if(Input.GetKeyUp(KeyCode.I))
        {
            _inventoryUI.Panel.InventoryScreen.SetActive (!_inventoryUI.Panel.InventoryScreen.activeSelf);            
        }

        if (Input.GetKeyUp (KeyCode.E))
        {
            _inventoryUI.Panel.EquipmentScreen.SetActive (!_inventoryUI.Panel.EquipmentScreen.activeSelf);            
        }

        if(Input.GetKeyUp (KeyCode.C))
        {
            _inventoryUI.Panel.AttributesScreen.SetActive (!_inventoryUI.Panel.AttributesScreen.activeSelf);
        }

        if(_inventoryUI.Panel.InventoryScreen.activeSelf)
        {
            _inventoryUI.Panel.InventroyToggleButton.SetActive (false);
        }
        else
        {
            _inventoryUI.Panel.InventroyToggleButton.SetActive (true);
        }

        if(_inventoryUI.Panel.EquipmentScreen.activeSelf)
        {
            _inventoryUI.Panel.EquipmentToggleButton.SetActive (false);
        }
        else
        {
            _inventoryUI.Panel.EquipmentToggleButton.SetActive (true);
        }

        if(_inventoryUI.Panel.AttributesScreen.activeSelf)
        {
            _inventoryUI.Panel.AttributesToggleButton.SetActive (false);
        }
        else
        {
            _inventoryUI.Panel.AttributesToggleButton.SetActive (true);
        }
    }    

    public void ToggleScreen(GameObject screen)
    {
        screen.SetActive (!screen.activeSelf);        
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if (PickUpItemOnCollision)
        {
            if (collision.gameObject.CompareTag ("Item"))
            {
                collision.GetComponent<ItemPickup> ().PickUp ();
            }
        }
    }
}
