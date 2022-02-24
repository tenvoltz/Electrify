using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int CheckCollision(ItemType itemType)
    {
        if (itemType == ItemType.Sphere) return CircleCollision();
        else if (itemType == ItemType.Rod) return BoxCollision();
        else return 0;
    }
    public bool isOverlapping(ItemType itemType)
    {
        return CheckCollision(itemType) > 0;
    }
    public int CircleCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale[0] / 2);
        return hitColliders.Length;
    }
    public int BoxCollision()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2);
        return hitColliders.Length;
    }
}
