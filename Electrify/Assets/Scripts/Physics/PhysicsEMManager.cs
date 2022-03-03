using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEMManager : MonoBehaviour
{
    public static float couloumbConstant = 1;
    public static float magneticPermeabilityConstant = 1;
    public static int rodSubdivisions = 10;
    //cannot access static variable directly, so this is created
    public float _coulombConstant = 1;
    public float _magneticPermeabilityConstant = 1;
    public int _rodSubdivisions = 10;
    private void OnValidate()
    {
        couloumbConstant = _coulombConstant;
        magneticPermeabilityConstant = _magneticPermeabilityConstant;
        rodSubdivisions = _rodSubdivisions;
    }


    private float timeInterval = 1f / 60;
    [SerializeField] private GameObject FaradayContainer;
    [SerializeField] private GameObject GilbertContainer;

    //private List<MovingParticle> movingParticleList;
    //private List<MovingRod> movingRodList;

    private List<Movable> movableObjectsList;
    private List<ElectricField> electricFieldList; //point, rod, plane, uniform
    private List<MagneticField> magneticFieldList; //point, wire, plane, uniform
    private List<GameObject> faradayList;
    private List<GameObject> gilbertList;
    void Start()
    {
        movableObjectsList = new List<Movable>(GetComponentsInChildren<Movable>());
        electricFieldList = new List<ElectricField>(GetComponentsInChildren<ElectricField>());
        magneticFieldList = new List<MagneticField>(GetComponentsInChildren<MagneticField>());

        faradayList = new List<GameObject>();
        if (FaradayContainer != null)
        {
            Transform faradayObjects = FaradayContainer.transform;
            for (int i = 0; i < faradayObjects.childCount; i++)
            {
                faradayList.Add(faradayObjects.GetChild(i).gameObject);
            }
        }
        gilbertList = new List<GameObject>();
        if (GilbertContainer != null)
        {
            Transform gilbertObjects = GilbertContainer.transform;
            for (int i = 0; i < gilbertObjects.childCount; i++)
            {
                gilbertList.Add(gilbertObjects.GetChild(i).gameObject);
            }
        }

        foreach (Movable movable in movableObjectsList)
        {
            StartCoroutine(Cycle(movable));
        }
    }

    public IEnumerator Cycle(Movable movable)
    {
        bool first = true;
        while (true)
        {
            if (first)
            {
                first = false;
                yield return new WaitForSeconds(Random.Range(0, timeInterval));
            }
            ApplyElectricForce(movable);
            ApplyMagneticForce(movable);
            yield return new WaitForSeconds(timeInterval);
        }
    }

    private void ApplyElectricForce(Movable movable)
    {
        if(movable.physicsObject.electricField is FiniteLineElectricField)
        {
            FiniteLineElectricField electricField1 = (FiniteLineElectricField)movable.physicsObject.electricField;
            foreach (FiniteLineElectricField.Segment segment in electricField1.GetSegments())
            {
                Vector3 combinedElectricField = Vector3.zero;
                foreach (ElectricField electricField2 in electricFieldList)
                {
                    if (electricField2.gameObject == movable.gameObject) continue;
                    if (faradayList.Count == 0) combinedElectricField += electricField2.GetField(segment.position);
                    else combinedElectricField += electricField2.GetExposedFieldFromFaraday(segment.position, faradayList);
                }
                movable.rb.AddForceAtPosition(segment.charge * combinedElectricField, segment.position);
            }
        }
        else if(movable.physicsObject.electricField is PointChargeElectricField)
        {
            Vector3 combinedElectricField = Vector3.zero;
            foreach (ElectricField electricField in electricFieldList)
            {
                if (electricField.gameObject == movable.gameObject) continue;
                if (faradayList.Count == 0) combinedElectricField += electricField.GetField(movable.transform.position);
                else combinedElectricField += electricField.GetExposedFieldFromFaraday(movable.transform.position, faradayList);
            }
            movable.rb.AddForce(movable.physicsObject.chargeable.charge * combinedElectricField);
        }
    }

    private void ApplyMagneticForce(Movable movable)
    {
        if (movable.physicsObject.electricField is FiniteLineElectricField) return;
        if (movable.physicsObject.magneticField is StraightWireMagneticField)
        {
            StraightWireMagneticField magneticField1 = (StraightWireMagneticField)movable.physicsObject.magneticField;
            foreach (StraightWireMagneticField.Segment segment in magneticField1.GetSegments())
            {
                Vector3 combinedMagneticField = Vector3.zero;
                foreach (MagneticField magneticField2 in magneticFieldList)
                {
                    if (magneticField2.gameObject == movable.gameObject) continue;
                    if (magneticField2 is StraightWireMagneticField)
                    {
                        if (gilbertList.Count == 0) combinedMagneticField += ((StraightWireMagneticField)magneticField2).GetFieldFromThisWireSegment(segment.position);
                        else combinedMagneticField += ((StraightWireMagneticField)magneticField2).GetExposedFieldThisWireSegmentFromGilbert(segment.position, gilbertList);
                    }
                    else
                    {
                        if (gilbertList.Count == 0) combinedMagneticField += magneticField2.GetField(segment.position);
                        else combinedMagneticField += magneticField2.GetExposedFieldFromGilbert(segment.position, gilbertList);
                    }
                }
                Vector3 combinedMagneticforce = Vector3.Cross(combinedMagneticField, segment.current * (segment.length*segment.direction));
                movable.rb.AddForceAtPosition(combinedMagneticforce, segment.position);
            }
        }
        else if (movable.physicsObject.electricField is PointChargeElectricField)
        {
            Vector3 combinedMagneticField = Vector3.zero;
            foreach (MagneticField magneticField in magneticFieldList)
            {
                if (magneticField.gameObject == movable.gameObject) continue;
                if (gilbertList.Count == 0) combinedMagneticField += magneticField.GetField(movable.transform.position);
                else combinedMagneticField += magneticField.GetExposedFieldFromGilbert(movable.transform.position, gilbertList);
            }
            //Unity uses a left-handed cross product. Need to flip F = qv x B -> F = B x qv for left-handed system.
            movable.rb.AddForce(Vector3.Cross(combinedMagneticField,movable.physicsObject.chargeable.charge * movable.rb.velocity));
        }
    }

    public void AddPhysicsObject(GameObject gameObject)
    {
        PhysicsObject physicsObject = gameObject.GetComponent<PhysicsObject>();
        if (physicsObject == null) return;
        if(physicsObject.electricField != null) electricFieldList.Add(physicsObject.electricField);
        if (physicsObject.movable != null)
        {
            movableObjectsList.Add(physicsObject.movable);
            StartCoroutine(Cycle(physicsObject.movable));
        }
    }

    public void DestroyPhysicsObject(GameObject gameObject)
    {
        PhysicsObject physicsObject = gameObject.GetComponent<PhysicsObject>();
        if (physicsObject == null) return;
        if (physicsObject.electricField != null) electricFieldList.Remove(physicsObject.electricField);
        if (physicsObject.movable != null)
        {
            movableObjectsList.Remove(physicsObject.movable);
            StopCoroutine(Cycle(physicsObject.movable));
        }
        Destroy(gameObject);
    }
}
