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
    private void Awake()
    {
        movable = GetComponent<Movable>();
        physicsObject = movable.physicsObject;
        movable.rb.constraints = RigidbodyConstraints.FreezePosition;
        movable.rb.centerOfMass = pivotFromCenterAt * physicsObject.GetLength() * 0.5f * physicsObject.GetDirection();
        movable.rb.useGravity = false;
    }

    private void OnValidate()
    {
        UpdatePivot();
    }

    public void UpdatePivot()
    {
        movable.rb.centerOfMass = pivotFromCenterAt * physicsObject.GetLength() * 0.5f * physicsObject.GetDirection();
        if (physicsObject != null)  physicsObject.UI.setPivot(movable.rb.centerOfMass);
    }

    private Vector3 GetPivot()
    {
        return pivotFromCenterAt * physicsObject.GetLength() * 0.5f * physicsObject.GetDirection();
    }


}
