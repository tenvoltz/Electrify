using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plane))]
[RequireComponent(typeof(MeshCollider))]

public class OldPlaneMagneticField : OldMagneticField
{
    [SerializeField] private GameObject uMFPrefab;
    [SerializeField] private int strength = 1;
    private MeshCollider c;
    public Vector3 direction = Vector3.zero;
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
    public override Vector3 GetDirection(Vector3 other)
    {
        return this.transform.TransformDirection(Vector3.up);
    }
    public override float GetStrength(Vector3 other)
    {
        return strength;
    }
    void Start()
    {
        if (GetComponent<MeshCollider>() != null) c = GetComponent<MeshCollider>();
        else c = gameObject.AddComponent<MeshCollider>();
        CreateMagneticField();
    }

    private void CreateMagneticField()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Bounds b = filter.mesh.bounds;

        GameObject magneticField = Instantiate(uMFPrefab, transform);
        BoxCollider bc = magneticField.GetComponent<BoxCollider>();
        bc.size = new Vector3(b.size.x, 100, b.size.z);
        OldUniformMagneticField mf = magneticField.GetComponent<OldUniformMagneticField>();
        mf.direction = filter.transform.TransformDirection(filter.mesh.normals[0]);
        mf.strength = strength;
    }
}
