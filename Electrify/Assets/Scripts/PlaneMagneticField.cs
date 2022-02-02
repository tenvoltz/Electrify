using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plane))]
public class PlaneMagneticField : MonoBehaviour
{
    [SerializeField] private GameObject uMFPrefab;
    [SerializeField] private int strength = 1;
    // Start is called before the first frame update
    void Start()
    {
        CreateMagneticField();
    }

    private void CreateMagneticField()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Bounds b = filter.mesh.bounds;

        GameObject magneticField = Instantiate(uMFPrefab, transform);
        BoxCollider bc = magneticField.GetComponent<BoxCollider>();
        bc.size = new Vector3(b.size.x, 100, b.size.z);
        UniformMagneticField mf = magneticField.GetComponent<UniformMagneticField>();
        mf.direction = filter.transform.TransformDirection(filter.mesh.normals[0]);
        mf.strength = strength;
    }
}
