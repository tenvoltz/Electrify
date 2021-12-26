using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plane))]
public class PlaneMagneticField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        createMagneticField();
    }

    private void createMagneticField()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        GameObject magneticField = new GameObject("Magnetic Field");
        magneticField.transform.parent = transform;
        magneticField.transform.localPosition = Vector3.zero;
        magneticField.transform.localEulerAngles = Vector3.zero;
        magneticField.transform.localScale = Vector3.one;

        Bounds b = filter.mesh.bounds;
        BoxCollider bc = magneticField.AddComponent<BoxCollider>();
        bc.size = new Vector3(b.size.x, 100, b.size.z);
        bc.isTrigger = true;

        MagneticField mf = magneticField.AddComponent<MagneticField>();
        mf.direction = filter.transform.TransformDirection(filter.mesh.normals[0]);
        mf.strength = 100;
    }
}
