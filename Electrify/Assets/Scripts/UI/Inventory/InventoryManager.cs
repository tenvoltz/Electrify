using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryListObject> inventoryList;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemButtonObject;
    [Header("Prefab")]
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject rodPrefab;
    private void Start()
    {
        inventoryList = GetComponent<InventoryList>().inventoryList;
        foreach(InventoryListObject item in inventoryList)
        {
            GenerateInventoryItem(item);
        }
    }

    private void GenerateInventoryItem(InventoryListObject inventoryItem)
    {
        GameObject itemButton = Instantiate(itemButtonObject, inventory.transform);
        InventorySlotButton slotButton = itemButton.GetComponent<InventorySlotButton>();
        RectTransform buttonRectTransform = itemButton.GetComponent<RectTransform>();
        Vector2 buttonRectCenter = buttonRectTransform.rect.center;
        Vector3 buttonCenter = buttonRectTransform.TransformPoint(new Vector3(buttonRectCenter.x, buttonRectCenter.y, -1));
        float buttonSize = Mathf.Min(buttonRectTransform.rect.width, buttonRectTransform.rect.height); //Get the button's smallest dimension
        GameObject item = null;
        if (inventoryItem.itemType == ItemType.Rod)
        {
            item = Instantiate(GetRodPrefab(), buttonCenter, this.transform.rotation, itemButton.transform);
            item.transform.localScale = new Vector3(buttonSize * 0.5f * 1.414f, buttonSize * 0.35f, buttonSize * 0.35f);
            item.transform.localRotation = Quaternion.Euler(0, 180, 135);
            ObjectUI UI = item.GetComponent<PhysicsObject>().UI;
            UI.UpdateSize(new Vector3(1f, 0.6f, 0.6f) * 2);
            UI.UpdateLocalScale(inventoryItem.size);
        }
        else
        {
            item = Instantiate(GetSpherePrefab(), buttonCenter, this.transform.rotation, itemButton.transform);
            item.transform.localScale = new Vector3(buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f);
            item.transform.localRotation = Quaternion.Euler(0, 180, 0);
            ObjectUI UI = item.GetComponent<PhysicsObject>().UI;
            UI.UpdateSize(Vector3.one * 2);
            UI.UpdateLocalScale(inventoryItem.size);
        }
        if (inventoryItem.chargeableObject)
        {
            Chargeable chargeable = item.AddComponent<Chargeable>();
            chargeable.particleType = inventoryItem.particleType;
            chargeable.magnitude = inventoryItem.magnitude;
            chargeable.UpdateCharge();
        }
        if (inventoryItem.movableObject)
        {
            Movable movable = item.AddComponent<Movable>();
            movable.mass = inventoryItem.mass;
            movable.UpdateMass();
        }
        if (inventoryItem.pivotableObject)
        {
            Pivotable pivotable = item.AddComponent<Pivotable>();
            pivotable.pivotFromCenterAt = inventoryItem.pivotFromCenterAt;
            pivotable.UpdatePivot();
        }
        slotButton.item = item;
        slotButton.itemProperty = inventoryItem;
    }
    private GameObject GetSpherePrefab()
    {
        if (spherePrefab == null) spherePrefab = Resources.Load("Prefabs/Sphere") as GameObject;
        return spherePrefab;
    }
    private GameObject GetRodPrefab()
    {
        if (rodPrefab == null) rodPrefab = Resources.Load("Prefabs/Rod") as GameObject;
        return rodPrefab;
    }
}
