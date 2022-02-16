using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class PlaneElectricField : ElectricField
{
    private MeshCollider c;

    public Vector3 direction = Vector3.zero;
    public override Vector3 GetDirection(Vector3 other)
    {
        return this.transform.TransformDirection(Vector3.up);
    }

    public override Vector3 GetField(Vector3 other)
    {
        direction = GetDirection(other);
        float distance = (this.transform.position - other).magnitude;
        Ray ray = new Ray(other, -1 * direction);
        RaycastHit hit;
        if (c.Raycast(ray, out hit, distance))
            return GetStrength(other) * direction;
        return Vector3.zero;
    }

    private void Start()
    {
        if (GetComponent<MeshCollider>() != null) c = GetComponent<MeshCollider>();
        else c = gameObject.AddComponent<MeshCollider>();
    }
}