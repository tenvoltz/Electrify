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
}
