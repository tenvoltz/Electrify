using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagneticField : MonoBehaviour
{
    [HideInInspector] public PhysicsObject physicsObject;
        
    public virtual void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
    }
    public abstract Vector3 GetField(Vector3 other);
}
