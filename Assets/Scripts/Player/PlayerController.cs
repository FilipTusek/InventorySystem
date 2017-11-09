using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool PickUpItemOnCollision = true;

    [Serializable]
    public struct InventoryPanel
    {
        public GameObject InventoryScreen;
        public GameObject EquipmentScreen;

        public GameObject InventroyToggleButton;
        public GameObject EquipmentToggleButton;
    }

    public InventoryPanel inventoryPanel;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    private void Start ( )
    {
        DisableInventoryPanels ();
    }

    private void Update ( )
    {
        CheckForScreenToggle ();
    }

    private void CheckForScreenToggle()
    {
        if(Input.GetKeyUp(KeyCode.I))
        {
            inventoryPanel.InventoryScreen.SetActive (!inventoryPanel.InventoryScreen.activeSelf);            
        }

        if (Input.GetKeyUp (KeyCode.E))
        {
            inventoryPanel.EquipmentScreen.SetActive (!inventoryPanel.EquipmentScreen.activeSelf);            
        }

        if(inventoryPanel.InventoryScreen.activeSelf)
        {
            inventoryPanel.InventroyToggleButton.SetActive (false);
        }
        else
        {
            inventoryPanel.InventroyToggleButton.SetActive (true);
        }

        if(inventoryPanel.EquipmentScreen.activeSelf)
        {
            inventoryPanel.EquipmentToggleButton.SetActive (false);
        }
        else
        {
            inventoryPanel.EquipmentToggleButton.SetActive (true);
        }
    }

    private void DisableInventoryPanels()
    {
        inventoryPanel.InventoryScreen.SetActive (false);        
        inventoryPanel.EquipmentScreen.SetActive (false);        
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
