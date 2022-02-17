using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ElectricField : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float strength = 1;
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
        return strength;
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Moving Particle"))
        {
            Rigidbody rb = other.attachedRigidbody;
            Particle p = other.gameObject.GetComponent<MovingParticle>();
            Vector3 electricField = GetStrength(other.transform.position) * GetDirection(other.transform.position);
            Vector3 force = p.charge * electricField;
            rb.AddForce(force);
        }
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
