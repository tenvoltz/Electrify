using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightWireMagneticField : MagneticField
{
    public float current = 1;
    public override Vector3 GetField(Vector3 other)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 distanceAlongWire = Vector3.Project(distance, physicsObject.GetDirection());
        if (distanceAlongWire.magnitude < physicsObject.GetLength() / 2)
        {
            Vector3 distanceFromWire = distanceAlongWire - this.transform.InverseTransformPoint(other);
            Vector3 field = Vector3.Cross(current * physicsObject.GetDirection(), distanceFromWire.normalized) / Mathf.Pow(distanceFromWire.magnitude, 2);
            field = (PhysicsEMManager.magneticPermeabilityConstant / (4 * Mathf.PI)) * field;
            return field;
        }
        return Vector3.zero;
    }
    public override Vector3 GetExposedFieldFromGilbert(Vector3 other, List<GameObject> gilbertObjects)
    {
        Vector3 distance = other - this.transform.position;
        Vector3 distanceAlongWire = Vector3.Project(distance, physicsObject.GetDirection());
        if (distanceAlongWire.magnitude < physicsObject.GetLength() / 2)
        {
            Vector3 distanceFromWire = distanceAlongWire - this.transform.InverseTransformPoint(other);
            if (IntersectGilbertCage(other, distanceFromWire.normalized, distanceFromWire.magnitude, gilbertObjects))
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 field = Vector3.Cross(current * physicsObject.GetDirection(), distanceFromWire.normalized) / Mathf.Pow(distanceFromWire.magnitude, 2);
                field = (PhysicsEMManager.magneticPermeabilityConstant / (4 * Mathf.PI)) * field;
                return field;
            }
        }
        return Vector3.zero;
    }

    public Vector3 GetFieldFromThisWireSegment(Vector3 other)
    {
        Vector3 field = Vector3.zero;
        Vector3 distance = other - this.transform.position;
        Vector3 distanceVectorAlongWire = Vector3.Project(distance, physicsObject.GetDirection());
        if (distanceVectorAlongWire.magnitude < physicsObject.GetLength() / 2)
        {
            Vector3 distanceVectorNormalToWire = distance - distanceVectorAlongWire;
            Vector3 dLength = (physicsObject.GetLength()/PhysicsEMManager.rodSubdivisions * physicsObject.GetDirection());
            field = Vector3.Cross(current * dLength, distanceVectorNormalToWire.normalized) / Mathf.Pow(distanceVectorNormalToWire.magnitude, 2);
            field = (PhysicsEMManager.magneticPermeabilityConstant / (4 * Mathf.PI)) * field;
        }
        return field;
    }
    public Vector3 GetExposedFieldThisWireSegmentFromGilbert(Vector3 other, List<GameObject> gilbertObjects)
    {
        Vector3 field = Vector3.zero;
        Vector3 distance = other - this.transform.position;
        Vector3 distanceVectorAlongWire = Vector3.Project(distance, physicsObject.GetDirection());
        if (distanceVectorAlongWire.magnitude < physicsObject.GetLength() / 2)
        {
            Vector3 distanceVectorNormalToWire = distance - distanceVectorAlongWire;
            if (IntersectGilbertCage(other, -distanceVectorNormalToWire.normalized, distanceVectorNormalToWire.magnitude, gilbertObjects))
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 dLength = (physicsObject.GetLength() / PhysicsEMManager.rodSubdivisions * physicsObject.GetDirection());
                field = Vector3.Cross(current * dLength, distanceVectorNormalToWire.normalized) / Mathf.Pow(distanceVectorNormalToWire.magnitude, 2);
                field = (PhysicsEMManager.magneticPermeabilityConstant / (4 * Mathf.PI)) * field;
            }
        }
        return field;
    }

    public IEnumerable<Segment> GetSegments()
    {
        for (int n = 0; n < PhysicsEMManager.rodSubdivisions; n++)
        {
            float segmentLength = physicsObject.GetLength() / PhysicsEMManager.rodSubdivisions;
            Vector3 segmentPosition = this.transform.position + ((n + 0.5f) * segmentLength - physicsObject.GetLength() / 2) * physicsObject.GetDirection();
            Vector3 segmentDirection = physicsObject.GetDirection();
            float segmentCurrent = current;
            yield return new Segment(segmentPosition, segmentDirection, segmentLength, segmentCurrent);
        }
    }
    public struct Segment
    {
        public static readonly Segment Empty;
        public bool isInstantiated;
        public Vector3 position;
        public Vector3 direction;
        public float length;
        public float current;
        public Segment(Vector3 _position, Vector3 _direction, float _length, float _current)
        {
            this.position = _position;
            this.direction = _direction;
            this.length = _length;
            this.current = _current;
            this.isInstantiated = true;
        }
        public override string ToString()
        {
            return this.isInstantiated + " " + this.position + " " + this.direction + " " + this.length + " " + this.current;
        }
    }











}
