using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEMManager : MonoBehaviour
{
    //Couloumb Constant is 5.4124265025 E 36
    [SerializeField] private float couloumbConstant = 541;
    private float timeInterval = 1f / 60;
    public int rodSubdivisions = 100;
    [SerializeField] private GameObject FaradayContainer;

    private List<MovingParticle> movingParticleList;
    private List<MovingRod> movingRodList;
    //ElectricField point, rod, plane, uniform, 
    //magnetic field point, plane, uniform, wire
    private List<ElectricField> electricFieldList; //to be removed later for specific interaction handling
    private List<PointChargeElectricField> electricParticleList;
    private List<FiniteLineElectricField> electricRodList;
    private List<PlaneElectricField> electricPlaneList;
    private List<UniformElectricField> electricUniformList;
    //private List<PointChargeMagneticField> magneticParticleList;
    //private List<UniformMagneticField> magneticPlaneList;
    //private List<UniformMagneticField> magneticUniformList;
    private List<MagneticField> magneticFieldList;
    private List<GameObject> faradayList;
    // Start is called before the first frame update
    void Start()
    {
        movingParticleList = new List<MovingParticle>(FindObjectsOfType<MovingParticle>());
        movingRodList = new List<MovingRod>(FindObjectsOfType<MovingRod>());
        electricFieldList = new List<ElectricField>(FindObjectsOfType<ElectricField>());
        magneticFieldList = new List<MagneticField>(FindObjectsOfType<MagneticField>());
        electricParticleList = new List<PointChargeElectricField>(FindObjectsOfType<PointChargeElectricField>());
        electricRodList = new List<FiniteLineElectricField>(FindObjectsOfType<FiniteLineElectricField>());
        electricPlaneList = new List<PlaneElectricField>(FindObjectsOfType<PlaneElectricField>());
        electricUniformList = new List<UniformElectricField>(FindObjectsOfType<UniformElectricField>());
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
            ApplyElectricForce(mr, rodSubdivisions);
            //ApplyMagneticForce(mp);
            yield return new WaitForSeconds(timeInterval);
        }
    }
    private bool IntersectsFaradayObject(Vector3 origin, Vector3 direction, float maxDistance)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(origin, direction, out hitInfo, maxDistance))
        {
            GameObject collidedGameObject = hitInfo.collider.gameObject;
            foreach(GameObject gameObject in faradayList)
            {
                if(collidedGameObject == gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ApplyElectricForce(MovingParticle mp)
    {
        Vector3 force = Vector3.zero;
        Vector3 eField = Vector3.zero;
        Vector3 position = mp.transform.position;
        Vector3 distance = Vector3.zero;

        foreach (PointChargeElectricField electricParticle in electricParticleList)
        {
            if (electricParticle.gameObject == mp.gameObject) continue;
            distance = electricParticle.transform.position - position;
            if (!IntersectsFaradayObject(position, distance.normalized, distance.magnitude))
            {
                eField = electricParticle.GetField(mp.transform.position);
                force = mp.charge * eField;
                mp.rb.AddForce(force);
            }
        }
        foreach (FiniteLineElectricField electricRod in electricRodList)
        {
            float dLength = electricRod.GetRodLength() / rodSubdivisions;
            Vector3 electricRodPosition = electricRod.transform.position;
            for (int n = -rodSubdivisions / 2; n <= rodSubdivisions / 2; n++)
            {
                Vector3 newPos = electricRodPosition + (n * dLength) * electricRod.GetRodDirection();
                distance = newPos - position;
                if (!IntersectsFaradayObject(position, distance.normalized, distance.magnitude))
                { //to be optimized later to integrate chunks instead of dL segments
                    eField = electricRod.GetField(position, n*dLength-dLength/2,n*dLength+dLength/2);
                    force = mp.charge * eField;
                    mp.rb.AddForce(force);
                }
            }
        }
        foreach (PlaneElectricField electricPlane in electricPlaneList)
        {
            distance = electricPlane.transform.position - position;
            if (!IntersectsFaradayObject(position, -1*electricPlane.GetDirection(position), distance.magnitude))
            {
                eField = electricPlane.GetField(position);
                force = mp.charge * eField;
                mp.rb.AddForce(force);
            }
        }
        foreach (UniformElectricField electricUniform in electricUniformList)
        {
            eField = electricUniform.GetField(position);
            force = mp.charge * eField;
            mp.rb.AddForce(force);
        }
    }
    private void ApplyElectricForce(MovingRod mr, int subdivisions)
    {
        Vector3 force = Vector3.zero;
        Vector3 eField = Vector3.zero;
        Vector3 position = mr.transform.position;
        Vector3 newPosition = Vector3.zero;
        Vector3 distance = Vector3.zero;
        Vector3 rodDirection = mr.GetDirection();
        float length = mr.GetLength();
        float dLength = length / subdivisions;

        foreach (PointChargeElectricField electricParticle in electricParticleList)
        {
            Vector3 otherPosition = electricParticle.transform.position;
            for (int n = -subdivisions / 2; n <= subdivisions / 2; n++)
            {
                newPosition = position + (n * dLength) * rodDirection;
                distance = otherPosition - newPosition;
                if (!IntersectsFaradayObject(newPosition, distance.normalized, distance.magnitude))
                {
                    eField = electricParticle.GetField(newPosition);
                    force = (mr.lambda * dLength) * eField;
                    mr.rb.AddForceAtPosition(force, newPosition);
                }
            }
        }
        foreach (FiniteLineElectricField electricRod in electricRodList)
        {
            if (electricRod.gameObject == mr.gameObject) continue;
            float otherdLength = electricRod.GetRodLength() / subdivisions;
            Vector3 otherPosition = electricRod.transform.position;
            for (int n = -subdivisions / 2; n <= subdivisions / 2; n++)
            {
                newPosition = position + (n * dLength) * rodDirection;
                for (int i = -subdivisions / 2; i <= subdivisions / 2; i++)
                {
                    Vector3 otherNewPos = otherPosition + (i * otherdLength) * electricRod.GetRodDirection();
                    distance = otherNewPos - newPosition;
                    if (!IntersectsFaradayObject(newPosition, distance.normalized, distance.magnitude))
                    { //to be optimized later to integrate chunks instead of dL segments
                        eField = electricRod.GetField(newPosition, i * otherdLength - otherdLength / 2, i * otherdLength + otherdLength / 2);
                        force = (mr.lambda * dLength) * eField;
                        mr.rb.AddForceAtPosition(force, newPosition);
                    }
                }
            }
        }
        foreach (PlaneElectricField electricPlane in electricPlaneList)
        {
            Vector3 otherPosition = electricPlane.transform.position;
            for (int n = -subdivisions / 2; n <= subdivisions / 2; n++)
            {
                newPosition = position + (n * dLength) * rodDirection;
                distance = otherPosition - newPosition;
                if (!IntersectsFaradayObject(newPosition, -1*electricPlane.GetDirection(newPosition), distance.magnitude))
                {
                    eField = electricPlane.GetField(newPosition);
                    force = (mr.lambda * dLength) * eField;
                    mr.rb.AddForceAtPosition(force, newPosition);
                }
            }
        }
        foreach (UniformElectricField electricUniform in electricUniformList)
        {
            eField = electricUniform.GetField(position);
            force = (mr.lambda * length) * eField;
            mr.rb.AddForce(force);
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
