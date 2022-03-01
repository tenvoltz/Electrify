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
    [HideInInspector] public MagneticField magneticField;
    [HideInInspector] public Conductable conductable;
    [HideInInspector] public Goalable goalable;
    [HideInInspector] public ObjectUI UI;
    public ItemType itemType;
    private void Awake()
    {
        GetUIReference();
    }
    private void OnEnable()
    {
        UpdateReference();
    }

    public void UpdateReference()
    {
        chargeable = GetComponent<Chargeable>();
        movable = GetComponent<Movable>();
        pivotable = GetComponent<Pivotable>();
        conductable = GetComponent<Conductable>();
        goalable = GetComponent<Goalable>();
        electricField = GetComponent<ElectricField>();
        magneticField = GetComponent<MagneticField>();
        InitReference();
    }
    public void InitReference()
    {
        chargeable?.Init();
        movable?.Init();
        pivotable?.Init();
        conductable?.Init();
        goalable?.Init();
        electricField?.Init();
        magneticField?.Init();
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (chargeable != null) UI.EnableChargeDisplay();
        else UI.DisableChargeDisplay();
        if (movable != null || pivotable != null) UI.DisableLock();
        else UI.EnableLock();
        if (pivotable != null) UI.EnablePivot();
        else UI.DisablePivot();
        if (conductable != null) UI.EnableConductorOutline();
        else UI.EnableInsulatorOutline();
        if (goalable != null) UI.EnableGoal();
        else UI.DisableGoal();
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
    private void GetUIReference()
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
}
