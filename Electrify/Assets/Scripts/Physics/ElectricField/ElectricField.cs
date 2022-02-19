using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ElectricField : MonoBehaviour
{
    public abstract Vector3 GetField(Vector3 other);
    public abstract Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects);
    public static bool IntersectFaradayCage(Vector3 origin, Vector3 direction, float maxDistance, List<GameObject> faradayObjects)
    {
        RaycastHit[] hitInfo = Physics.RaycastAll(origin, direction, maxDistance);
        if (hitInfo.Length == 0) return false;
        foreach (RaycastHit hit in hitInfo)
        {
            GameObject collidedGameObject = hit.collider.gameObject;
            foreach (GameObject gameObject in faradayObjects)
            {
                if (collidedGameObject == gameObject) return true;
            }
        }
        return false;
    }
}
/*
    public virtual void Render();
*/