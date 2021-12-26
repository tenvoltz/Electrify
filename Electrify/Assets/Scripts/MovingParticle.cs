using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParticle : Particle
{
    private float electronMass = 0.000549f;
    private float protonMass = 1.007316f;

    [HideInInspector] public Rigidbody rb;
    public float mass;
    // Start is called before the first frame update
    void Start()
    {
        UpdateSurface();
        mass = charge > 0 ? charge * protonMass : -charge * electronMass;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;
    }
}
