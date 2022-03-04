using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class InventoryListObject
{
    public ItemType itemType;
    public Vector3 size;
    public bool chargeableObject;
    public ParticleType particleType;
    public float magnitude;
    public ContactType contactType;
    public bool movableObject;
    public float mass;
    public bool pivotableObject;
    public float pivotFromCenterAt;
    public bool goalableObject;
    public GoalDetector goal;
}
public enum ItemType
{
    Sphere = 1,
    Rod = 2,
}
public enum ContactType
{
    Conductor = 1,
    Insulator = 2
}
//Thanks ForceX for the code
public class InventoryList : MonoBehaviour
{
    //This is our list we want to use to represent our class as an array.
    public List<InventoryListObject> inventoryList = new List<InventoryListObject>(1);


    void AddNew()
    {
        //Add a new index position to the end of our list
        inventoryList.Add(new InventoryListObject());
    }

    void Remove(int index)
    {
        //Remove an index position from our list at a point in our list array
        inventoryList.RemoveAt(index);
    }
}