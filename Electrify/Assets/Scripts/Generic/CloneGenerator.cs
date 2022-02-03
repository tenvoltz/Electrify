using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneGenerator : MonoBehaviour
{
    public int rowCount = 1;
    public int columnCount = 1;
    public Vector3 rowDirection = Vector3.right;
    public Vector3 columnDirection = Vector3.forward;
    public Vector3 randomOffset = Vector3.zero;

    private List<Vector3> GetPoints()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                Vector3 newPoint = rowDirection * j + columnDirection * i;
                points.Add(newPoint + GetRandomOffset());
             }
        }
        points.RemoveAt(0); //Duplicated of the original object
        return points;
    }

    private Vector3 GetRandomOffset()
    {
        return new Vector3(Random.Range(-randomOffset.x, randomOffset.x),
                           Random.Range(-randomOffset.y, randomOffset.y),
                           Random.Range(-randomOffset.z, randomOffset.z));
    }

    void Start()
    {
        foreach (Vector3 point in GetPoints())
        {
            GameObject clone = Instantiate(gameObject, transform.position + point, transform.rotation) as GameObject;
            clone.transform.parent = gameObject.transform.parent;
            Destroy(clone.GetComponent<CloneGenerator>());
        }
        Destroy(this);
    }
}
