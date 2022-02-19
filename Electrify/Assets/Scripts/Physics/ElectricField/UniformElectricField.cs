using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformElectricField : ElectricField
{
    public Vector3 direction;
    public float magnitude;
    public override Vector3 GetField(Vector3 other)
    {
        return magnitude * direction;
    }
    public override Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects)
    {
        if (IntersectFaradayCage(other, -direction, float.MaxValue, faradayObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
    }
}
