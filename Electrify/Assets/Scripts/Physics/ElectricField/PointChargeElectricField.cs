using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointChargeElectricField : ElectricField
{
    [SerializeField] private float k = 1;
    [SerializeField] private float charge = 1;

    public override Vector3 GetDirection(Vector3 other)
    {
        return (other - this.transform.position).normalized;
    }
    public override float GetStrength(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        return k * charge / Mathf.Pow(distance.magnitude,2);
    }

    public override Vector3 GetField(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 field = (k * charge / Mathf.Pow(distance.magnitude, 3)) * distance;
        return field;
    }

    public override Color GetColor(Vector3 position)
    {
        if (isInside(position)) return color * 50 * GetStrength(position);
        else return Color.clear;
    }
}