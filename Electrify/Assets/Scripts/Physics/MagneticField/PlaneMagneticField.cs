using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]

public class PlaneMagneticField : MagneticField
{
    private MeshCollider c;
    public float strength = 1;
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 direction = GetPlaneNormal();
        float distance = (this.transform.position - other).magnitude;
        Ray ray = new Ray(other, -1 * direction);
        RaycastHit hit;
        if (c.Raycast(ray, out hit, distance))
            return strength * direction;
        return Vector3.zero;
    }
    private void Awake()
    {
        if (GetComponent<MeshCollider>() != null) c = GetComponent<MeshCollider>();
        else c = gameObject.AddComponent<MeshCollider>();
    }
    public Vector3 GetPlaneNormal()
    {
        return this.transform.TransformDirection(Vector3.up);
    }
}