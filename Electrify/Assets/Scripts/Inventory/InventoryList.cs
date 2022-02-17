using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class InventoryListObject
{
    public ItemType itemType;
    public Particle.ParticleType particleType;
    public bool movable;
    public float magnitude;
    public float mass;
}
public enum ItemType
{
    Particle = 1,
    Rod = 2,
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