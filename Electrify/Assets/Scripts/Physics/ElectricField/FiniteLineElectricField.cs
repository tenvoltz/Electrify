using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteLineElectricField: ElectricField
{
    /*
     *                                           O           X
     *                                     a                 X h
     *                              XXXXXXXXXXXXXX           X
     *      -------------------------------------------------
     *      -------------------------------------------------
     *                              0
     * */

    [SerializeField] private float k = 1;
    [SerializeField] private float lambda = 1;

    public Vector3 direction = Vector3.zero;
    public override Vector3 GetDirection(Vector3 other)
    {
        return GetField(other).normalized;
    }
    public override float GetStrength(Vector3 other)
    {
        return GetField(other).magnitude;
    }
    public Vector3 GetRodDirection()
    {
        return this.transform.TransformDirection(Vector3.right);
    }
    public float GetRodLength()
    {
        return this.transform.localScale[0];
    }
    public float GetLambda()
    {
        return lambda;
    }
    
    public override Vector3 GetField(Vector3 other)
    {
        float length = GetRodLength();
        direction = GetRodDirection();
        Vector3 distance = other - this.transform.position;
        Vector3 a = Vector3.Dot(direction, distance) * direction.normalized;
        Vector3 h = distance - a;
        float fieldXStrength = 0;
        float fieldYStrength = 0;
        fieldXStrength = 1 / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(length - a.magnitude - length/2,2));
        fieldXStrength = fieldXStrength - 1 / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(a.magnitude + length / 2, 2));
        fieldXStrength = fieldXStrength * k * lambda;
        if(h.magnitude != 0)
        {
            fieldYStrength = ((length - a.magnitude - length / 2) / h.magnitude) / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(length - a.magnitude - length / 2, 2));
            fieldYStrength = fieldYStrength + ((a.magnitude + length / 2) / h.magnitude) / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(a.magnitude + length / 2, 2));
            fieldYStrength = fieldYStrength * k * lambda;
        }
        Vector3 fieldX = a.normalized * Mathf.Abs(fieldXStrength);
        Vector3 fieldY = h.normalized * Mathf.Abs(fieldYStrength);
        Vector3 field = fieldX + fieldY;
        return field;
    }

    public Vector3 GetField(Vector3 other, float lowBoundL, float upBoundL)
    {
        float length = upBoundL - lowBoundL;
        direction = this.transform.TransformDirection(Vector3.right);
        Vector3 newCenter = this.transform.position + (lowBoundL + length / 2) * direction;
        Vector3 distance = other - newCenter;
        Vector3 a = Vector3.Dot(direction, distance) * direction.normalized;
        Vector3 h = distance - a;
        float fieldXStrength = 0;
        float fieldYStrength = 0;
        System.Func<float, float> antiDerivativeX = l => k*lambda / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(l - a.magnitude, 2));
        fieldXStrength = antiDerivativeX(length/2) - antiDerivativeX(-length / 2);
        if (h.magnitude != 0)
        {
            System.Func<float, float> antiDerivativeY = l => (k*lambda)*((l - a.magnitude) / h.magnitude) / Mathf.Sqrt(Mathf.Pow(h.magnitude, 2) + Mathf.Pow(l - a.magnitude, 2));
            fieldYStrength = antiDerivativeY(length / 2) - antiDerivativeY(-length / 2);
        }
        Vector3 fieldX = a.normalized * Mathf.Abs(fieldXStrength);
        Vector3 fieldY = h.normalized * Mathf.Abs(fieldYStrength);
        Vector3 field = fieldX + fieldY;
        return field;
    }

    public override Color GetColor(Vector3 position)
    {
        if (isInside(position)) return color * 50 * GetStrength(position);
        else return Color.clear;
    }
}