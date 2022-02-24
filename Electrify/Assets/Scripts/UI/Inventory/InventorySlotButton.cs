using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotButton : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public GraphicRaycaster graphicRaycaster;
    [HideInInspector] public InventoryManager inventoryManager;
    [HideInInspector] public InventoryListObject inventoryItem;
    [HideInInspector] public GameObject item;
    private static PhysicsEMManager physicsEMManager;
    [Header("Prefab")]
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject rodPrefab;
    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Draggable draggable = item.AddComponent<Draggable>();
        draggable.slotButton = this;
        draggable.inventoryManager = inventoryManager;
    }

    public void GenerateInventoryItem()
    {
        if (inventoryItem.itemType == ItemType.Rod) item = Instantiate(GetRodPrefab()); 
        else item = Instantiate(GetSpherePrefab());
        item.GetComponent<PhysicsObject>().itemType = inventoryItem.itemType;
        SetDimensionInButtonMode(item);
        if (inventoryItem.chargeableObject)
        {
            Chargeable chargeable = item.AddComponent<Chargeable>();
            chargeable.particleType = inventoryItem.particleType;
            chargeable.magnitude = inventoryItem.magnitude;
            chargeable.UpdateCharge();
            if (inventoryItem.itemType == ItemType.Sphere) item.AddComponent<PointChargeElectricField>();
            else if (inventoryItem.itemType == ItemType.Rod) item.AddComponent<FiniteLineElectricField>();
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

    public void SetDimensionInButtonMode(GameObject item)
    {
        PhysicsObject physicsObject = item.GetComponent<PhysicsObject>();
        item.transform.SetParent(this.transform, true);
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        float buttonSize = Mathf.Min(buttonRectTransform.rect.width, buttonRectTransform.rect.height); //Get the button's smallest dimension
        item.transform.localPosition = new Vector3(0,0,-1);
        if (physicsObject.itemType == ItemType.Rod) //Doubt we will have rod in invetory
        {
            item.transform.localScale = new Vector3(buttonSize * 0.5f * 1.414f, buttonSize * 0.4f, buttonSize * 0.4f);
            item.transform.localRotation = Quaternion.Euler(0, 180, 135);
            ObjectUI UI = physicsObject.UI;
            UI.UpdateSize(new Vector3(1f, 0.5f, 0.5f) * 2);
        }
        else
        {
            item.transform.localScale = new Vector3(buttonSize * 0.5f, buttonSize * 0.5f, buttonSize * 0.5f);
            item.transform.localRotation = Quaternion.Euler(0, 180, 0);
            ObjectUI UI = physicsObject.UI;
            UI.UpdateSize(Vector3.one * 2);
        }
    }

    public void RetrieveInventoryItem(GameObject _item)
    {
        if (item != _item) return;
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        Vector3 buttonCenter = buttonRectTransform.TransformPoint(buttonRectTransform.rect.center);
        LeanTween.move(item, buttonCenter, 1f).setIgnoreTimeScale(true).setEaseOutCubic().setOnComplete(() => { SetDimensionInButtonMode(item); });
    }
}
