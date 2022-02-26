using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public class Movable : MonoBehaviour
{
    [HideInInspector] public PhysicsObject physicsObject;
    [HideInInspector] public Rigidbody rb;
    public float mass = 1;
    public Vector3 initialVelocity;
    public void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        else rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.mass = mass;
        rb.velocity = initialVelocity;
        rb.useGravity = false;
    }

    public void UpdateMass()
    {
        if (rb != null) rb.mass = mass;
    }

    private void OnValidate()
    {
        if (mass < 0) mass = 0;
        UpdateMass();
    }
}
