using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEMManager : MonoBehaviour
{
    //Couloumb Constant is 5.4124265025 E 36
    [SerializeField] private float couloumbConstant = 541;
    private float timeInterval = 1f / 60;

    private List<MovingParticle> mpList;
    private List<MovingRod> mrList;
    private List<Particle> pList;
    //ElectricField point, rod, plane, uniform, 
    //magnetic field point, plane, uniform, wire
    private List<ElectricField> eList; //to be removed later for specific interaction handling
    private List<FiniteLineElectricField> erList;
    private List<MagneticField> mList;
    private List<GameObject> fList;
    // Start is called before the first frame update
    void Start()
    {
        mpList = new List<MovingParticle>(FindObjectsOfType<MovingParticle>());
        mrList = new List<MovingRod>(FindObjectsOfType<MovingRod>());
        pList = new List<Particle>(FindObjectsOfType<Particle>());
        eList = new List<ElectricField>(FindObjectsOfType<ElectricField>());
        mList = new List<MagneticField>(FindObjectsOfType<MagneticField>());
        fList = new List<GameObject>();
        erList = new List<FiniteLineElectricField>(FindObjectsOfType<FiniteLineElectricField>());
        if(GameObject.Find("Faraday Objects") != null)
        {
            Transform faradayObjects = GameObject.Find("Faraday Objects").transform;
            for (int i = 0; i < faradayObjects.childCount; i++)
            {
                    fList.Add(faradayObjects.GetChild(i).gameObject);
            }
        }
        foreach (MovingParticle mp in mpList)
        {
            StartCoroutine(Cycle(mp));
        }
        foreach(MovingRod mr in mrList)
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
            ApplyElectricForce(mr, 100);
            //ApplyMagneticForce(mp);
            yield return new WaitForSeconds(timeInterval);
        }
    }
    private void ApplyElectricForce(MovingParticle mp)
    {
        Vector3 force = Vector3.zero;
        foreach (ElectricField field in eList)
        {
            Vector3 eField = field.GetField(mp.transform.position);
            force = mp.charge * eField;
            mp.rb.AddForce(force);
        }
    }
    private void ApplyElectricForce(MovingRod mr, int subdivisions)
    {
        Vector3 force = Vector3.zero;
        Vector3 pos = mr.transform.position;
        foreach (ElectricField field in eList)
        {
            if (field.gameObject == mr.gameObject)
            {
                continue;
            }
            float dLength = mr.GetLength() / subdivisions;
            for (int n = -subdivisions/2; n <= subdivisions/2; n++)
            {
                Vector3 newPos = pos + (n * dLength) * mr.GetDirection();
                Vector3 dEField = field.GetField(newPos);
                force = (mr.lambda*dLength)*dEField;
                mr.rb.AddForceAtPosition(force,newPos);
            }
        }
    }
    private void ApplyMagneticForce(MovingParticle mp)
    {
        Vector3 force = Vector3.zero;
        foreach (MagneticField field in mList)
        {
            Vector3 fieldVector = field.GetStrength(mp.transform.position) * field.GetDirection(mp.transform.position);
            force = mp.charge * Vector3.Cross(mp.rb.velocity, fieldVector);
            mp.rb.AddForce(force);
        }
    }
}
