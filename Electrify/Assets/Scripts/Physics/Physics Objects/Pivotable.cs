using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class Pivotable : MonoBehaviour
{
    private PhysicsObject physicsObject;
    private Movable movable;
    [Range(-1, 1)] 
    public float pivotFromCenterAt;
    public void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
        movable = physicsObject.movable;
        movable.rb.constraints = RigidbodyConstraints.FreezePosition;
        movable.rb.centerOfMass = GetPivot();
        movable.rb.useGravity = false;
        UpdatePivot();
    }
    public void UpdatePivot()
    {
        if (movable != null) movable.rb.centerOfMass = GetPivot();
        if (physicsObject != null) physicsObject.UI.setPivot(movable.rb.centerOfMass);
    }

    private Vector3 GetPivot()
    {
        return pivotFromCenterAt * physicsObject.GetLength() * 0.5f * Vector3.right;
    }

    private void OnValidate()
    {
        UpdatePivot();
    }


}
