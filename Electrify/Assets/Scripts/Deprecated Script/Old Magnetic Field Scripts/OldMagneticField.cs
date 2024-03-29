using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OldMagneticField : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Color color = Color.white;

    [HideInInspector] public List<FieldArrow> faList;
    private Collider c;

    private void Awake()
    {
        c = GetComponent<Collider>();
    }
    private void Start()
    {
        faList = new List<FieldArrow>();
        Render();
    }
    public virtual Vector3 GetDirection(Vector3 other)
    {
        return Vector3.one;
    }
    public virtual float GetStrength(Vector3 other)
    {
        return 0;
    }

    public virtual Vector3 GetField(Vector3 other)
    {
        return Vector3.one;
    }
    public virtual Color GetColor(Vector3 position)
    {
        if (isInside(position)) return color;
        else return Color.clear;
    }
    public bool isInside(Vector3 position)
    {
        return (c.ClosestPoint(position) - position).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
    }

    public virtual void Render()
    {
        GameObject fieldArrow = Instantiate(arrowPrefab, transform);
        faList.Add(fieldArrow.GetComponent<FieldArrow>());
    }
}
