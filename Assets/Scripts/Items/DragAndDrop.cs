using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerClickHandler
{
    private Transform _playerTransform;
    private Transform _slotTransform;

    private GameObject _inventoryScreen;
    private GameObject _equipmentScreen;    

    private bool _isDragged = false;

    private GameManager _gameManager;
    

    private void Awake ( )
    {
        _playerTransform = GameObject.Find ("Player").transform;
        _inventoryScreen = GameObject.Find ("InventoryPanel");
        _equipmentScreen = GameObject.Find ("EquipmentPanel");
    }

    private void Start ( )
    {
        _gameManager = GameManager.instance;
        _slotTransform = transform.parent.parent.transform;
    }

    private void Update ( )
    {
        if(_isDragged)
        {
            transform.position = Input.mousePosition;            
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }
    }

    public void DragOrDrop()
    {
        if (!_gameManager.ItemBeingDragged)
        {
            if (!_isDragged)
            {
                _isDragged = true;
                _gameManager.ItemBeingDragged = true;
                transform.SetParent (transform.root.transform);
            }            
        }
        else
        {
            if (_isDragged)
            {
                _isDragged = false;
                _gameManager.ItemBeingDragged = false;
                transform.SetParent (_slotTransform);
            }
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        DragOrDrop ();
    }
}
