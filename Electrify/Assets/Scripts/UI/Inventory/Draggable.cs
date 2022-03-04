using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public InventoryManager inventoryManager;
    [HideInInspector] public InventorySlotButton slotButton;
    private static PhysicsEMManager physicsEMManager;
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = this.transform.position.z;
        return mousePosition;
    }
    public PhysicsEMManager GetPhysicsEMManager()
    {
        if(physicsEMManager == null)
            physicsEMManager = FindObjectOfType<PhysicsEMManager>();
        return physicsEMManager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.parent = GetPhysicsEMManager().PhysicsObjectContainer.gameObject.transform;
        this.transform.localScale = slotButton.inventoryItem.size;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = GetMousePosition();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        bool isHoverOnWires = false;
        foreach (BarbedWire barbedWire in slotButton.barbedWireList)
        {
            isHoverOnWires = isHoverOnWires || barbedWire.isHoverOnWire();
        }
        if (isHoverOnWires || slotButton.isHoverOnButton() || isOverlapping(this.GetComponent<PhysicsObject>().itemType))
        {
            slotButton.RetrieveInventoryItem(this.gameObject);
        }
        else
        {
            Vector3 position = this.transform.position;
            position.z = 0; //All objects lie on z = 0 plane
            this.transform.position = position;
            GetComponent<Collider>().enabled = true;
            GetComponent<Conductable>()?.Init();
            physicsEMManager.AddPhysicsObject(this.gameObject);
            Destroy(slotButton.gameObject);
            inventoryManager.UpdateLayoutGroup();
        }
        Destroy(this);
    }

    private bool isHoverOnButton()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData pointer = new PointerEventData(null);
        pointer.position = Input.mousePosition;
        slotButton.graphicRaycaster.Raycast(pointer, hits);
        foreach (RaycastResult hit in hits)
        {
            GameObject collidedGameObject = hit.gameObject;
            Debug.Log(hit.gameObject);
            if (collidedGameObject.transform.parent == slotButton.transform) return true;
            foreach(BarbedWire barbedWire in slotButton.barbedWireList)
            {
                if (collidedGameObject.transform.parent == barbedWire.transform) return true;
            }
        }
        return false;
    }
    public bool isOverlapping(ItemType itemType)
    {
        return CheckCollision(itemType) > 0;
    }
    public int CheckCollision(ItemType itemType)
    {
        if (itemType == ItemType.Sphere) return CircleCollision();
        else if (itemType == ItemType.Rod) return BoxCollision();
        else return 0;
    }
    public int CircleCollision()
    {
        Vector3 position = this.transform.position;
        position.z = 0; //All objects lie on z = 0 plane
        Collider[] hitColliders = Physics.OverlapSphere(position, transform.localScale[0] / 2);
        return hitColliders.Length;
    }
    public int BoxCollision()
    {
        Vector3 position = this.transform.position;
        position.z = 0; //All objects lie on z = 0 plane
        Collider[] hitColliders = Physics.OverlapBox(position, transform.localScale / 2, transform.rotation);
        return hitColliders.Length;
    }
}
