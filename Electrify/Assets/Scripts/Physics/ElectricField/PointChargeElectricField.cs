using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Chargeable))]
public class PointChargeElectricField : ElectricField
{
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 field = (PhysicsEMManager.couloumbConstant * chargeable.charge / Mathf.Pow(distance.magnitude, 3)) * distance;
        return field;
    }
    public override Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects)
    {
        Vector3 pointToOther = other - this.transform.position;
        if (IntersectFaradayCage(other, -pointToOther.normalized, pointToOther.magnitude, faradayObjects))
        {
            return Vector3.zero;
        }
        else return GetField(other);
    }
}