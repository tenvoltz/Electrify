using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class MagneticField : MonoBehaviour
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Moving Particle"))
        {
            Rigidbody rb = other.attachedRigidbody;
            Particle p = other.gameObject.GetComponent <MovingParticle> ();
            Vector3 forceDirection = Vector3.Cross(rb.velocity, GetDirection(other.transform.position));
            Vector3 force = forceDirection * p.charge * GetStrength(other.transform.position);
            rb.AddForce(force);
        }
    }
    public virtual Vector3 GetDirection(Vector3 other)
    {
        return Vector3.one;
    }
    public virtual float GetStrength(Vector3 other)
    {
        return strength;
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