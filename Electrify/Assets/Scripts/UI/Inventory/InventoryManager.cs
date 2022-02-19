using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryListObject> inventoryList;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemButtonObject;
    [Header("Prefab")]
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private GameObject movingParticlePrefab;
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
        RectTransform buttonRectTransform = itemButton.GetComponent<RectTransform>();
        Vector2 buttonRectCenter = buttonRectTransform.rect.center;
        Vector3 buttonCenter = buttonRectTransform.TransformPoint(new Vector3(buttonRectCenter.x, buttonRectCenter.y, -1));
        if (inventoryItem.itemType == ItemType.Particle)
        {
            if (inventoryItem.movable)
            {
                GameObject item = Instantiate(movingParticlePrefab, buttonCenter, this.transform.rotation, itemButton.transform);
                MovingParticle movingParticle = item.GetComponent<MovingParticle>();
                movingParticle.particleType = inventoryItem.particleType;
                movingParticle.magnitude = inventoryItem.magnitude;
                movingParticle.mass = inventoryItem.mass;
                item.transform.localScale = Vector3.one * buttonRectTransform.rect.width * 0.5f;
                item.GetComponent<TrailRenderer>().enabled = false;
            }
            else
            {
                GameObject item = Instantiate(particlePrefab, buttonCenter, this.transform.rotation, itemButton.transform);
                Particle particle = item.GetComponent<Particle>();
                particle.particleType = inventoryItem.particleType;
                particle.magnitude = inventoryItem.magnitude;
                item.transform.localScale = Vector3.one * buttonRectTransform.rect.width * 0.5f;
                item.GetComponent<TrailRenderer>().enabled = false;
            }
        }
    }
}
