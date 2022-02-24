using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public static GameObject UIPrefab;
    [HideInInspector] public Chargeable chargeable;
    [HideInInspector] public Movable movable;
    [HideInInspector] public Pivotable pivotable;
    [HideInInspector] public ElectricField electricField;
    [HideInInspector] public ObjectUI UI;
    public ItemType itemType;
    private void Awake()
    {
        if (GetComponentInChildren<ObjectUI>() == null)
        {
            GameObject UIObject = Instantiate(GetUIPrefab(), transform.position, transform.rotation) as GameObject;
            UIObject.transform.SetParent(transform, true);
            UI = UIObject.GetComponent<ObjectUI>();
        }
        else UI = GetComponentInChildren<ObjectUI>();
        UI.UpdateSize(this.transform.localScale);
        UI.UpdatePosition(this.transform.localScale);
    }
    private void Start()
    {
        chargeable = GetComponent<Chargeable>();
        movable = GetComponent<Movable>();
        pivotable = GetComponent<Pivotable>();
        electricField = GetComponent<ElectricField>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (chargeable != null)
        {
            UI.EnableChargeDisplay();
            chargeable.UpdateCharge();
        }
        
        if (movable != null || pivotable != null)
        {
            UI.DisableLock();
        }

        if (pivotable != null)
        {
            UI.EnablePivot();
            pivotable.UpdatePivot();
        }
    }

    public Vector3 GetDirection()
    {
        return this.transform.TransformDirection(Vector3.right);
    }
    public float GetLength()
    {
        return this.transform.localScale[0];
    }//Implements GetHeight(){return localScale[1]} and GetWidth(){return localScale[2]} if needed
    private GameObject GetUIPrefab()
    {
        if (UIPrefab == null) UIPrefab = Resources.Load("Prefabs/ObjectCanvas") as GameObject;
        return UIPrefab;
    }
}
