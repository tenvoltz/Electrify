using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEMManager : MonoBehaviour
{
    public static float couloumbConstant = 1;
    public static int rodSubdivisions = 10;
    public float _coulombConstant = 1; //cannot access static variable directly
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
    //ElectricField point, rod, plane, uniform, 
    //Magnetic field point, plane, uniform, wire
    private List<ElectricField> electricFieldList;
    //private List<PointChargeElectricField> electricParticleList;
    //private List<FiniteLineElectricField> electricRodList;
    //private List<PlaneElectricField> electricPlaneList;
    //private List<UniformElectricField> electricUniformList;

    //private List<PointChargeMagneticField> magneticParticleList;
    //private List<UniformMagneticField> magneticPlaneList;
    //private List<UniformMagneticField> magneticUniformList;
    private List<MagneticField> magneticFieldList;
    private List<GameObject> faradayList;
    void Start()
    {
        movingParticleList = new List<MovingParticle>(FindObjectsOfType<MovingParticle>());
        movingRodList = new List<MovingRod>(FindObjectsOfType<MovingRod>());
        magneticFieldList = new List<MagneticField>(FindObjectsOfType<MagneticField>());
        //electricParticleList = new List<PointChargeElectricField>(FindObjectsOfType<PointChargeElectricField>());
        ////electricRodList = new List<FiniteLineElectricField>(FindObjectsOfType<FiniteLineElectricField>());
        //electricPlaneList = new List<PlaneElectricField>(FindObjectsOfType<PlaneElectricField>());
        //electricUniformList = new List<UniformElectricField>(FindObjectsOfType<UniformElectricField>());
        electricFieldList = new List<ElectricField>(FindObjectsOfType<ElectricField>());

        faradayList = new List<GameObject>();
        if (FaradayContainer != null)
        {
            Transform faradayObjects = FaradayContainer.transform;
            for (int i = 0; i < faradayObjects.childCount; i++)
            {
                faradayList.Add(faradayObjects.GetChild(i).gameObject);
            }
        }
        foreach (MovingParticle mp in movingParticleList)
        {
            StartCoroutine(Cycle(mp));
        }
        foreach(MovingRod mr in movingRodList)
        {
            StartCoroutine(Cycle(mr));
        }
    }
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
}
