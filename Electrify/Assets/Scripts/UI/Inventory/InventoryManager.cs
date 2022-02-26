using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryListObject> inventoryList;
    [SerializeField] private GameObject inventory;
    private static GameObject itemSlotPrefab;

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
        GameObject itemButton = Instantiate(GetSlotPrefab(), inventory.transform);
        InventorySlotButton slotButton = itemButton.GetComponent<InventorySlotButton>();
        slotButton.inventoryManager = this;
        slotButton.inventoryItem = inventoryItem;
        slotButton.GenerateInventoryItem();
    }
    public void UpdateLayoutGroup()
    {
        StartCoroutine(UpdateLayoutGroupCoroutine());
    }
    public IEnumerator UpdateLayoutGroupCoroutine()
    {
        yield return new WaitForEndOfFrame();
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 initialDelta = rectTransform.sizeDelta;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        Vector2 finalDelta = rectTransform.sizeDelta;
        GetComponent<ContentSizeFitter>().enabled = false;
        rectTransform.sizeDelta = initialDelta;
        LeanTween.value(this.gameObject, initialDelta.y, finalDelta.y, 1f).setIgnoreTimeScale(true).setEaseOutCubic()
            .setOnUpdate((float val) => {
                rectTransform.sizeDelta = new Vector2(finalDelta.x, val);
            }).setOnComplete(() => { GetComponent<ContentSizeFitter>().enabled = true; });
    }

    private GameObject GetSlotPrefab()
    {
        if (itemSlotPrefab == null) itemSlotPrefab = Resources.Load("Prefabs/InventorySlot") as GameObject;
        return itemSlotPrefab;
    }
}
