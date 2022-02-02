using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMagneticField : MagneticField
{
    public Vector3 direction = Vector3.zero;
    public override Vector3 GetDirection(Vector3 other)
    {
        return Vector3.Cross(other - this.transform.position,direction);
    }
    public override float GetStrength(Vector3 other)
    {
        return strength / (2 * Mathf.PI * Vector3.Magnitude(GetDirection(other)));
    }

    public override Color GetColor(Vector3 position)
    {
        if (isInside(position))
        {
            Color output = (Color.white - color) * 50 * GetStrength(position);
            output.a = 1;
            return output;
        }
        else return Color.clear;
    }
}
