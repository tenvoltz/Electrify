using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParticle : Particle
{
    private float electronMass = 0.549f;
    private float protonMass = 1.007316f;
    private float neutronMass = 1.008665f;

    [HideInInspector] public Rigidbody rb;
    public float mass = 0;
    public Vector3 initialVelocity;
    // Start is called before the first frame update
    void Start()
    {
        if(mass != 0)
        {
            switch (particleType)
            {
                case ParticleType.Proton:
                    mass = magnitude * protonMass;
                    break;
                case ParticleType.Electron:
                    mass = magnitude * electronMass;
                    break;
                case ParticleType.Neutron:
                    mass = neutronMass;
                    break;
            }
        }
        UpdateSurface();
        UpdateSize();
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        else rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.velocity = initialVelocity;
        rb.useGravity = false;
        rb.freezeRotation = true;
    }
    void UpdateSize()
    {
        float radius = Mathf.Pow(mass, 0.3333f);
        transform.localScale = new Vector3(1, 1, 1) * radius;
    }

    public void SetFreezingState(bool isFreeze)
    {
        rb.constraints = isFreeze? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotation;
    }
}
