using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotButton : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public InventoryListObject itemProperty;
    [HideInInspector] public GameObject item;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this);
    }
}
