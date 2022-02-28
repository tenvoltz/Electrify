using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingParticle))]
public class OldPointChargeMagneticField : OldMagneticField
{
    private MovingParticle mp;
    private Rigidbody rb;
    [SerializeField] public float u = 1;
    void Awake()
    {
        mp = this.GetComponent<MovingParticle>();
        rb = mp.GetComponent<Rigidbody>();
    }
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 field = Vector3.Cross(mp.charge * rb.velocity, distance.normalized) / Mathf.Pow(distance.magnitude, 2);
        field = (u / (4 * Mathf.PI)) * field;
        return field;
    }
    public override Vector3 GetDirection(Vector3 other)
    {
        return GetField(other).normalized;    
    }
    public override float GetStrength(Vector3 other)
    {
        return GetField(other).magnitude;
    }
}
