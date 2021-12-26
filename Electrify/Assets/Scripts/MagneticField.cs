using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MagneticField : MonoBehaviour
{   
    public Vector3 direction = Vector3.zero;
    public float strength = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Moving Particle"))
        {
            Rigidbody rb = other.attachedRigidbody;
            Particle p = other.gameObject.GetComponent <MovingParticle> ();
            Vector3 forceDirection = Vector3.Cross(rb.velocity, direction);
            Vector3 force = forceDirection * p.charge;
            rb.AddForce(force);
        }
    }

}
