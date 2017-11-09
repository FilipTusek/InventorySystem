using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckIfPointerOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DragAndDropManager _dragAndDropManager;

    private void Start ( )
    {
        _dragAndDropManager = DragAndDropManager.instance;
    }
    
    public void OnPointerEnter (PointerEventData eventData)
    {
        _dragAndDropManager.DropItem = false;
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        _dragAndDropManager.DropItem = true;
    }
}
