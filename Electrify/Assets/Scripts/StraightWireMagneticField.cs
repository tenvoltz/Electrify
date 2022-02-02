using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightWireMagneticField : MonoBehaviour
{
    [SerializeField] private GameObject cMFPrefab;
    [SerializeField] private int current = 1;
    void Start()
    {
        CreateMagneticField();
    }

    private void CreateMagneticField()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Bounds b = filter.mesh.bounds;

        GameObject magneticField = Instantiate(cMFPrefab, transform);
        BoxCollider bc = magneticField.GetComponent<BoxCollider>();
        bc.size = new Vector3(b.size.x*100, b.size.y, b.size.z*100);
        CircularMagneticField mf = magneticField.GetComponent<CircularMagneticField>();
        mf.direction = transform.TransformDirection(Vector3.up);
        mf.strength = current;
    }
}
