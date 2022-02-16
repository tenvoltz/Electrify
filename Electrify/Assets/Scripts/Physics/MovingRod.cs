using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingRod : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    private Collider c;
    public float mass = 1;
    public Vector3 initialVelocity;
    public Vector3 orientation;
    public float lambda = 1;
    public float length = 1;

    public Vector3 GetDirection()
    {
        orientation = this.transform.TransformDirection(Vector3.right);
        return orientation;
    }

    public float GetLength()
    {
        length = this.transform.localScale[0];
        return length;
    }
    void Start()
    {
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        else rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.velocity = initialVelocity;
        rb.useGravity = false;
        rb.freezeRotation = false;
    }

}
