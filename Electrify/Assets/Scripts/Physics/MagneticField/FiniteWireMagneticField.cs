using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteWireMagneticField : MagneticField
{
    [SerializeField] private int current = 1;
    private float u = 1;
    public Vector3 direction = Vector3.zero;
    public Vector3 GetField(Vector3 other, int subdivisions)
    {
        Vector3 distance = other - this.transform.position; 
        Vector3 distanceAlongWire = Vector3.Project(distance, GetWireDirection());
        if (distanceAlongWire.magnitude < GetWireLength() / 2)
        {
            Vector3 distanceFromWire = distanceAlongWire - other;
            Vector3 dLength = (GetWireLength()/subdivisions)*GetWireDirection();
            Vector3 field = Vector3.Cross(current * dLength, distanceFromWire.normalized) / Mathf.Pow(distanceFromWire.magnitude,2);
            field = (u / (4 * Mathf.PI)) * field;
            return field;
        }
        return Vector3.zero;
    }
    public override Vector3 GetDirection(Vector3 other)
    {
        return GetField(other).normalized;
    }
    public override float GetStrength(Vector3 other)
    {
        return GetField(other).magnitude;
    }
    public Vector3 GetWireDirection()
    {
        return this.transform.TransformDirection(Vector3.right);
    }
    public float GetWireLength()
    {
        return this.transform.localScale[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
