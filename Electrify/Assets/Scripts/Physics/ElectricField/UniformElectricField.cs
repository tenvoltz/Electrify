using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformElectricField : ElectricField
{
    public override Vector3 GetField(Vector3 other)
    {
        return chargeable.charge * physicsObject.GetDirection();
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
