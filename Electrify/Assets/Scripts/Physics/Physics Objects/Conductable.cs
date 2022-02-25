using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Chargeable))]
[RequireComponent(typeof(MeshFilter))]
public class Conductable : MonoBehaviour
{
    private Chargeable chargeable;
    private Mesh objectMesh;
    private float meshSurfaceArea;
    private float radius;
    public float temporaryCharge;
    private void Awake()
    {
        chargeable = GetComponent<Chargeable>();
        objectMesh = GetComponent<MeshFilter>().mesh;
        meshSurfaceArea = CalculateSurfaceArea(objectMesh,this.transform.localScale);
        radius = Mathf.Sqrt(meshSurfaceArea / (4 * Mathf.PI));
        switch (chargeable.particleType)
        {
            case ParticleType.Proton: temporaryCharge = chargeable.magnitude; break;
            case ParticleType.Neutron: temporaryCharge = 0; break;
            case ParticleType.Electron: temporaryCharge = -1 * chargeable.magnitude; break;
            default: Debug.Log("Something has gone wrong", this); break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Conductable collided = collision.gameObject.GetComponent<Conductable>();
        if(collided != null)
        {
            float otherRadius = collided.getRadius();
            float otherCharge = collided.temporaryCharge;
            float totalCharge = this.temporaryCharge + otherCharge;
            float thisResultantCharge = totalCharge / (otherRadius / radius + 1);

            if(thisResultantCharge > 0) chargeable.particleType = ParticleType.Proton;
            else if(thisResultantCharge < 0) chargeable.particleType = ParticleType.Electron;
            else chargeable.particleType = ParticleType.Neutron;

            chargeable.magnitude = Mathf.Abs(thisResultantCharge);
            chargeable.UpdateCharge();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        temporaryCharge = chargeable.charge;
    }

    public float getRadius()
    {
        return radius;
    }

    private float CalculateSurfaceArea(Mesh mesh, Vector3 scale)
    {
        Vector3[] meshVertices = mesh.vertices;
        int[] triangleIndices = mesh.triangles;
        float surfaceArea = 0;
        for(int i = 0; i < triangleIndices.Length; i += 3)
        {
            Vector3 sideA = meshVertices[triangleIndices[i + 1]] - meshVertices[triangleIndices[i]];
            Vector3 sideB = meshVertices[triangleIndices[i + 2]] - meshVertices[triangleIndices[i]];
            sideA = Vector3.Scale(sideA, scale);
            sideB = Vector3.Scale(sideB, scale);
            float triangleArea = (0.5f)*Vector3.Cross(sideA,sideB).magnitude;
            surfaceArea += triangleArea;
        }
        return surfaceArea;
    }
}
