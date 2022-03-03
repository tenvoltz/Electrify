using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformMagneticField : MagneticField
{
    public float strength = 1;

    public override Vector3 GetField(Vector3 other)
    {
        return strength * physicsObject.GetDirection();
    }
    public override Vector3 GetExposedFieldFromGilbert(Vector3 other, List<GameObject> gilbertObjects)
    {
        if (IntersectGilbertCage(other, -physicsObject.GetDirection(), float.MaxValue, gilbertObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
    }
}
