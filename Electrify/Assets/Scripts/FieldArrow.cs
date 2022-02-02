using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldArrow : MonoBehaviour
{
    public Vector3 direction = Vector3.one;
    public Color color = Color.black;
    private void Update()
    {
        if (direction == Vector3.up)
        {
            transform.rotation = Quaternion.Euler(270f, 0, 0);
        }
        else if (direction == Vector3.down)
        {
            transform.rotation = Quaternion.Euler(90f, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
