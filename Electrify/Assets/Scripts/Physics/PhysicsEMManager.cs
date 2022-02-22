using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEMManager : MonoBehaviour
{
    public static float couloumbConstant = 1;
    public static int rodSubdivisions = 10;
    //cannot access static variable directly, so this is created
    public float _coulombConstant = 1; 
    public int _rodSubdivisions = 10;
    private void OnValidate()
    {
        couloumbConstant = _coulombConstant;
        rodSubdivisions = _rodSubdivisions;
    }


    private float timeInterval = 1f / 60;
    [SerializeField] private GameObject FaradayContainer;

    private List<MovingParticle> movingParticleList;
    private List<MovingRod> movingRodList;

    private List<Movable> movableObjects;
    //ElectricField point, rod, plane, uniform, 
    private List<ElectricField> electricFieldList;

    //private List<PointChargeMagneticField> magneticParticleList;
    //private List<UniformMagneticField> magneticPlaneList;
    //private List<UniformMagneticField> magneticUniformList;
    //Magnetic field point, plane, uniform, wire
    private List<MagneticField> magneticFieldList;
    private List<GameObject> faradayList;
    void Start()
    {
        //movingParticleList = new List<MovingParticle>(FindObjectsOfType<MovingParticle>());
        //movingRodList = new List<MovingRod>(FindObjectsOfType<MovingRod>());

        movableObjects = new List<Movable>(FindObjectsOfType<Movable>());
        electricFieldList = new List<ElectricField>(FindObjectsOfType<ElectricField>());
        magneticFieldList = new List<MagneticField>(FindObjectsOfType<MagneticField>());

        faradayList = new List<GameObject>();
        if (FaradayContainer != null)
        {
            Transform faradayObjects = FaradayContainer.transform;
            for (int i = 0; i < faradayObjects.childCount; i++)
            {
                faradayList.Add(faradayObjects.GetChild(i).gameObject);
            }
        }
        /*
        foreach (MovingParticle mp in movingParticleList)
        {
            StartCoroutine(Cycle(mp));
        }
        foreach(MovingRod mr in movingRodList)
        {
            StartCoroutine(Cycle(mr));
        }*/

        foreach(Movable movable in movableObjects)
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
            //ApplyMagneticForce(movable);
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
        else
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

    /*
    public IEnumerator Cycle(MovingParticle mp)
    {
        bool first = true;
        while (true)
        {
            if (first)
            {
                first = false;
                yield return new WaitForSeconds(Random.Range(0, timeInterval));
            }
            ApplyElectricForce(mp);
            ApplyMagneticForce(mp);
            yield return new WaitForSeconds(timeInterval);
        }
    }
    public IEnumerator Cycle(MovingRod mr)
    {
        bool first = true;
        while (true)
        {
            if (first)
            {
                first = false;
                yield return new WaitForSeconds(Random.Range(0, timeInterval));
            }
            ApplyElectricForce(mr);
            //ApplyMagneticForce(mp);
            yield return new WaitForSeconds(timeInterval);
        }
    }
    private void ApplyElectricForce(MovingParticle mp)
    {
        Vector3 combinedElectricField = Vector3.zero;
        foreach(ElectricField electricField in electricFieldList)
        {
            if (electricField.gameObject == mp.gameObject) continue;
            if (faradayList.Count == 0) combinedElectricField += electricField.GetField(mp.transform.position);
            else combinedElectricField += electricField.GetExposedFieldFromFaraday(mp.transform.position, faradayList);
        }
        mp.rb.AddForce(mp.charge * combinedElectricField);
    }
    private void ApplyElectricForce(MovingRod mr)
    {
        foreach (FiniteLineElectricField.Segment segment in mr.electricRod.GetSegments())
        {
            Vector3 combinedElectricField = Vector3.zero;
            foreach (ElectricField electricField in electricFieldList)
            {
                if (electricField.gameObject == mr.gameObject) continue;
                if (faradayList.Count == 0) combinedElectricField += electricField.GetField(segment.position);
                else combinedElectricField += electricField.GetExposedFieldFromFaraday(segment.position, faradayList);
            }
            mr.rb.AddForceAtPosition(segment.charge * combinedElectricField, segment.position);
        }
    }

    private void ApplyMagneticForce(MovingParticle mp)
    {
        Vector3 force = Vector3.zero;
        foreach (MagneticField field in magneticFieldList)
        {
            Vector3 fieldVector = field.GetStrength(mp.transform.position) * field.GetDirection(mp.transform.position);
            force = mp.charge * Vector3.Cross(mp.rb.velocity, fieldVector);
            mp.rb.AddForce(force);
        }
    }
    */
}
