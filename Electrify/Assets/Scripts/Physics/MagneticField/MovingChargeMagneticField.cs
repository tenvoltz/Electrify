using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
[RequireComponent(typeof(Chargeable))]

public class MovingChargeMagneticField : MagneticField
{
    [HideInInspector] public Chargeable chargeable;
    [HideInInspector] public Movable movable;
    [HideInInspector] private Rigidbody rb;
    public override void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
        chargeable = physicsObject.chargeable;
        movable = physicsObject.movable;
        rb = movable.rb;
    }

    public override Vector3 GetField(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 field = Vector3.zero;
        if (Vector3.Project(distance, rb.velocity) == Vector3.zero)
        {
            field = Vector3.Cross(chargeable.charge * rb.velocity, distance.normalized) / Mathf.Pow(distance.magnitude, 2);
            field = (PhysicsEMManager.magneticPermeabilityConstant / (4 * Mathf.PI)) * field;
        }
        return field;
    }
}
