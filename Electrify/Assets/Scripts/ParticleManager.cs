using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Couloumb Constant is 5.4124265025 E 36
    [SerializeField] private float couloumbConstant = 541;
    private float timeInterval = 1f / 60;

    private List<MovingParticle> mpList;
    private List<Particle> pList;
    // Start is called before the first frame update
    void Start()
    {
        mpList = new List<MovingParticle>(FindObjectsOfType<MovingParticle>());
        pList = new List<Particle>(FindObjectsOfType<Particle>());
        foreach(MovingParticle mp in mpList)
        {
            StartCoroutine(Cycle(mp));
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
            applyElectricForce(mp);
            yield return new WaitForSeconds(timeInterval);
        }
    }
    private void applyElectricForce(MovingParticle mp)
    {
        Vector3 force = Vector3.zero;
        foreach (Particle p in pList)
        {
            if (mp == p) continue;
            float distance = Vector3.Distance(p.transform.position, mp.transform.position);
            if (distance == 0) continue;
            float magnitude = couloumbConstant * mp.charge * p.charge / (distance * distance);
            Vector3 direction = (mp.transform.position - p.transform.position).normalized;
            force += direction * magnitude * timeInterval;
        }
        mp.rb.AddForce(force);
    }
}
