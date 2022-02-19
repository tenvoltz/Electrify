using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformMagneticField :  MagneticField
{
    public Vector3 direction = Vector3.zero;
    public int strength = 1;

    [Header("Space between Arrow")]
    [SerializeField] private float xSpace = 1f;
    [SerializeField] private float zSpace = 1f;

    public override Vector3 GetField(Vector3 other)
    {
        return strength * this.transform.TransformDirection(Vector3.up);
    }
    public override Vector3 GetDirection(Vector3 other)
    {
        return this.transform.TransformDirection(Vector3.up);
    } 
    public override float GetStrength(Vector3 other)
    {
        return strength;
    }
    public override Color GetColor(Vector3 position)
    {
        if (isInside(position)) return color;
        else return Color.clear;
    }
    public override void Render()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        int xAmount = (int)(bc.size.x / xSpace);
        int zAmount = (int)(bc.size.z / zSpace);
        for(int zIndex = 0; zIndex < zAmount; zIndex++)
        {
            for (int xIndex = 0; xIndex < xAmount; xIndex++)
            {
                GameObject arrow = Instantiate(arrowPrefab, transform);
                Vector3 position = new Vector3(-bc.size.x*0.5f, 0, -bc.size.z*0.5f);
                position += Vector3.right * xSpace * (xIndex+0.5f) + Vector3.forward * zSpace * (zIndex+0.5f);
                arrow.transform.localPosition = position;
                FieldArrow fa = arrow.GetComponent<FieldArrow>();
                fa.direction = direction;
                faList.Add(fa);
            }
        }
    }
}
