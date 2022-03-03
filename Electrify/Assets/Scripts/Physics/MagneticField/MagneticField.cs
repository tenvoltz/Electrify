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

    public abstract Vector3 GetExposedFieldFromGilbert(Vector3 other, List<GameObject> gilbertObjects);

    public static bool IntersectGilbertCage(Vector3 origin, Vector3 direction, float maxDistance, List<GameObject> gilbertObjects)
    {
        RaycastHit[] hitInfo = Physics.RaycastAll(origin, direction, maxDistance);
        if (hitInfo.Length == 0) return false;
        foreach (RaycastHit hit in hitInfo)
        {
            GameObject collidedGameObject = hit.collider.gameObject;
            foreach (GameObject gameObject in gilbertObjects)
            {
                if (collidedGameObject == gameObject) return true;
            }
        }
        return false;
    }
}
