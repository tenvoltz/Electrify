using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformElectricField : ElectricField
{
    public Vector3 worldDirection;
    public float strength = 1;
    public override Vector3 GetField(Vector3 other)
    {
        return strength * worldDirection;
    }
    public override Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects)
    {
        if (IntersectFaradayCage(other, -physicsObject.GetDirection(), float.MaxValue, faradayObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
    }
}
