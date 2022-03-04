using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(Chargeable))]
public class FiniteLineElectricField: ElectricField
{
    /*
     *                                           O           X
     *                             distanceAlongRod          X distanceNormalToRod
     *                              XXXXXXXXXXXXXX           X
     *      -------------------------------------------------
     *      -------------------------------------------------
     *                              0
     *      
     * */
    public override Vector3 GetField(Vector3 other)
    {
        return GetField(other, this.transform.position, physicsObject.GetDirection(), physicsObject.GetLength(), chargeable.charge);
    }
    public override Vector3 GetExposedFieldFromFaraday(Vector3 other, List<GameObject> faradayObjects)
    {
        bool intersected = false;
        Vector3 exposedField = Vector3.zero;
        Segment startingSegment = default(Segment), endingSegment = default(Segment);
        foreach (Segment segment in GetSegments())
        {
            Vector3 pointToOther = other - segment.position;
            if (IntersectFaradayCage(other, -pointToOther.normalized, pointToOther.magnitude, faradayObjects))
            {
                intersected = true;
                if (startingSegment.isInstantiated && endingSegment.isInstantiated)
                {
                    Segment combinedSegment = CombiningSegment(startingSegment, endingSegment);
                    exposedField += GetField(other, combinedSegment.position, combinedSegment.direction, combinedSegment.length, combinedSegment.charge);
                }
                startingSegment = default(Segment);
                endingSegment = default(Segment);
            }
            else
            {
                if (!startingSegment.isInstantiated) startingSegment = segment;
                endingSegment = segment;
            }
        }
        if (!intersected) exposedField = GetField(other);
        else //Get the segment, if exist, that has not hit faraday cage that need to be compute
        {
            if (startingSegment.isInstantiated && endingSegment.isInstantiated)
            {
                Segment combinedSegment = CombiningSegment(startingSegment, endingSegment);
                exposedField += GetField(other, combinedSegment.position, combinedSegment.direction, combinedSegment.length, combinedSegment.charge);
            }
        }
        return exposedField;
    }
    public static Segment CombiningSegment(Segment startingSegment, Segment endingSegment)
    {
        float segmentLength = Vector3.Distance(startingSegment.position, endingSegment.position) + startingSegment.length;
        float segmentCharge = startingSegment.charge * segmentLength / startingSegment.length;
        Vector3 segmentPosition = (startingSegment.position + endingSegment.position) * 0.5f;
        Vector3 segmentDirection = startingSegment.direction;
        return new Segment(segmentPosition, segmentDirection, segmentLength, segmentCharge);
    }
    public Vector3 GetField(Vector3 other, Vector3 center, Vector3 direction, float length, float charge)
    {
        Vector3 distance = other - center;
        Vector3 distanceVectorAlongRod = Vector3.Dot(direction, distance) * direction.normalized;
        Vector3 distanceVectorNormalToRod = distance - distanceVectorAlongRod;

        float distanceAlongRod = distanceVectorAlongRod.magnitude;
        float distanceNormalToRod = distanceVectorNormalToRod.magnitude;
        float distanceToLeftEdgeOfRod = Mathf.Sqrt(Mathf.Pow(distanceNormalToRod, 2) + Mathf.Pow(length * 0.5f + distanceAlongRod, 2));
        float distanceToRightEdgeOfRod = Mathf.Sqrt(Mathf.Pow(distanceNormalToRod, 2) + Mathf.Pow(length * 0.5f - distanceAlongRod, 2));

        float fieldXStrength = 0;
        float fieldYStrength = 0;
        fieldXStrength = 1 / distanceToRightEdgeOfRod - 1 / distanceToLeftEdgeOfRod;
        fieldXStrength *= PhysicsEMManager.couloumbConstant * charge / length;
        if (distanceNormalToRod != 0)
        {
            fieldYStrength = (length * 0.5f - distanceAlongRod) / (distanceNormalToRod * distanceToRightEdgeOfRod);
            fieldYStrength += (length * 0.5f + distanceAlongRod) / (distanceNormalToRod * distanceToLeftEdgeOfRod);
            fieldYStrength *= PhysicsEMManager.couloumbConstant * charge / length;
        }
        Vector3 fieldX = distanceVectorAlongRod.normalized * fieldXStrength;
        Vector3 fieldY = distanceVectorNormalToRod.normalized * fieldYStrength;
        Vector3 field = fieldX + fieldY;
        return field;
    }
    public IEnumerable<Segment> GetSegments()
    {
        for (int n = 0; n < PhysicsEMManager.rodSubdivisions; n++)
        {
            float segmentLength = physicsObject.GetLength() / PhysicsEMManager.rodSubdivisions;
            float segmentCharge = chargeable.charge * segmentLength / physicsObject.GetLength();
            Vector3 segmentPosition = this.transform.position + ((n + 0.5f) * segmentLength - physicsObject.GetLength() / 2) * physicsObject.GetDirection();
            Vector3 segmentDirection = physicsObject.GetDirection();
            yield return new Segment(segmentPosition, segmentDirection, segmentLength, segmentCharge);
        }
    }
    public struct Segment
    {
        public static readonly Segment Empty;
        public bool isInstantiated;
        public Vector3 position;
        public Vector3 direction;
        public float length;
        public float charge;
        public Segment(Vector3 _position, Vector3 _direction, float _length, float _charge)
        {
            this.position = _position;
            this.direction = _direction;
            this.length = _length;
            this.charge = _charge;
            this.isInstantiated = true;
        }
        public override string ToString()
        {
            return this.isInstantiated + " " + this.position + " " + this.direction + " " + this.length + " " + this.charge;
        }
    }
}