using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCircularMagneticField : OldMagneticField
{
    public Vector3 direction = Vector3.zero;
    public int strength = 1;
    public override Vector3 GetDirection(Vector3 other)
    {
        return Vector3.Cross(direction, other - this.transform.position).normalized;
    }
    public override float GetStrength(Vector3 other)
    {
        float distance = Vector3.Cross(direction, other - this.transform.position).magnitude;
        return strength / (2 * Mathf.PI * distance);
    }

    public override Color GetColor(Vector3 position)
    {
        if (isInside(position)) return color * 50 * GetStrength(position);
        else return Color.clear;
    }
}
