using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Chargeable))]
[RequireComponent(typeof(MeshFilter))]
public class Conductable : MonoBehaviour
{
    private PhysicsObject physicsObject;
    private Chargeable chargeable;
    private float meshSurfaceArea;
    private float radius;
    private float temporaryCharge;
    public void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
        chargeable = physicsObject.chargeable;
        meshSurfaceArea = CalculateSurfaceArea(GetComponent<MeshFilter>().mesh, this.transform.localScale);
        radius = Mathf.Sqrt(meshSurfaceArea / (4 * Mathf.PI)); //Assume equipotential surface of that of a sphere
        temporaryCharge = chargeable.charge;
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
