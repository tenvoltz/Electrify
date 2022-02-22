using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]

public class PlaneElectricField : ElectricField
{
    private MeshCollider c;
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 direction = GetPlaneNormal();
        float distance = (this.transform.position - other).magnitude;
        Ray ray = new Ray(other, -1 * direction);
        RaycastHit hit;
        if (c.Raycast(ray, out hit, distance))
            return chargeable.charge * direction;
        return Vector3.zero;
    }
    public override Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects)
    {
        Vector3 pointToOther = other - this.transform.position;
        if (IntersectFaradayCage(other, -GetPlaneNormal(), pointToOther.magnitude, faradayObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
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
























/*
public override void Render()
{
    [Header("Space between Arrow")]
    [SerializeField] private float xSpace = 1f;
    [SerializeField] private float zSpace = 1f;
    BoxCollider bc = GetComponent<BoxCollider>();
    int xAmount = (int)(bc.size.x / xSpace);
    int zAmount = (int)(bc.size.z / zSpace);
    for (int zIndex = 0; zIndex < zAmount; zIndex++)
    {
        for (int xIndex = 0; xIndex < xAmount; xIndex++)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform);
            Vector3 position = new Vector3(-bc.size.x * 0.5f, 0, -bc.size.z * 0.5f);
            position += Vector3.right * xSpace * (xIndex + 0.5f) + Vector3.forward * zSpace * (zIndex + 0.5f);
            arrow.transform.localPosition = position;
            FieldArrow fa = arrow.GetComponent<FieldArrow>();
            fa.direction = direction;
            faList.Add(fa);
        }
    }
}
*/