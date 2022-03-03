using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum FieldType
{
    [Description("No Field")]
    None = 0,
    [Description("Electric Field")]
    Electric = 1,
    [Description("Magnetic Field")]
    Magnetic = 2
}
public class FieldDisplay : MonoBehaviour
{
    public FieldType fieldType;
    public int numRows = 1;
    public int numCols = 1;
    public float length = 1;
    public float width = 1;
    public float strengthCap = 1;
    private float unitLength;
    private float unitWidth;
    [Range(0.0f, 1.0f)] public float majoritySwitchThreshold = 1;
    private List<Vector3> fieldPoints;
    private List<GameObject> arrowObjects;
    [SerializeField] private Sprite pointingArrow;
    [SerializeField] private Sprite outArrow;
    [SerializeField] private Sprite inArrow;

    private List<ElectricField> electricFieldList;
    private List<MagneticField> magneticFieldList;
    [SerializeField] private GameObject FaradayContainer;
    [SerializeField] private GameObject GilbertContainer;
    private List<GameObject> faradayList;
    private List<GameObject> gilbertList;
    Gradient gradient;
    void Start()
    {
        unitLength = length / numRows;
        unitWidth = width / numCols;
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
        gilbertList = new List<GameObject>();
        if (GilbertContainer != null)
        {
            Transform gilbertObjects = GilbertContainer.transform;
            for (int i = 0; i < gilbertObjects.childCount; i++)
            {
                gilbertList.Add(gilbertObjects.GetChild(i).gameObject);
            }
        }
        setFieldPoints();
        setArrowObjects();

        gradient = new Gradient();
        Color orange = new Color(255, 141, 0, 1);
        //Color indigo = new Color(75, 0, 130);
        //Color violet = new Color(118, 1, 136);
        //Color[] rainbow = { Color.red, orange, Color.yellow, Color.green, Color.blue, indigo, violet };
        Color[] rainbow = { Color.blue, Color.green, Color.yellow, orange, Color.red };
        GradientColorKey[] colorKey = new GradientColorKey[rainbow.Length];
        for (int c = 0; c < rainbow.Length; c++)
        {
            colorKey[c].color = rainbow[c];
            colorKey[c].time = c / (rainbow.Length - 1.0f);
        }
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;
        gradient.SetKeys(colorKey, alphaKey);
    }
    private void Update()
    {
        //debugDisplayBounds();
        //debugDisplayPoints();
        //debugDisplayGrid();
        if (fieldType == FieldType.Electric) displayElectricFieldObjects(electricFieldList, faradayList);
        else if (fieldType == FieldType.Magnetic) displayMagneticFieldObjects(magneticFieldList, gilbertList);
        else displayNoFieldObjects();
    }

    private Color getColorFromFieldStrength(float magnitude)
    {
        float time = Mathf.InverseLerp(0, strengthCap, magnitude);
        Color arrowColor = gradient.Evaluate(time);
        return arrowColor;
    }

    private void displayNoFieldObjects()
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void displayElectricFieldObjects(List<ElectricField> electricFieldList, List<GameObject> faradayList)
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            Vector3 combinedElectricField = Vector3.zero;
            foreach (ElectricField electricField in electricFieldList)
            {
                if (faradayList.Count == 0) combinedElectricField += electricField.GetField(fieldPoints[p]);
                else combinedElectricField += electricField.GetExposedFieldFromFaraday(fieldPoints[p], faradayList);
            }
            if (combinedElectricField.magnitude == 0)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
                continue;
            }
            Vector3 electricFieldProjOntoPlaneNorm = Vector3.Dot(combinedElectricField, getPlaneNormal()) * getPlaneNormal();
            Vector3 electricFieldProjAlongPlane = combinedElectricField - electricFieldProjOntoPlaneNorm;
            arrowObjects[p].GetComponent<SpriteRenderer>().color = getColorFromFieldStrength(combinedElectricField.magnitude);
            if (electricFieldProjAlongPlane.magnitude * majoritySwitchThreshold >= electricFieldProjOntoPlaneNorm.magnitude)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = pointingArrow;
                Quaternion toRotate = Quaternion.LookRotation(electricFieldProjAlongPlane, this.transform.up);
                arrowObjects[p].transform.rotation = toRotate;
                arrowObjects[p].transform.Rotate(90,0,0,Space.Self);
            }
            else
            {
                if(Vector3.Angle(getPlaneNormal(), electricFieldProjOntoPlaneNorm) == 0)
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = outArrow;
                }
                else
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = inArrow;
                }
            }
        }
    }

    private void displayMagneticFieldObjects(List<MagneticField> magneticFieldList, List<GameObject> gilbertList)
    {
        for (int p = 0; p < fieldPoints.Count; p++)
        {
            Vector3 combinedMagneticField = Vector3.zero;
            foreach (MagneticField magneticField in magneticFieldList)
            {
                if (gilbertList.Count == 0) combinedMagneticField += magneticField.GetField(fieldPoints[p]);
                else combinedMagneticField += magneticField.GetExposedFieldFromGilbert(fieldPoints[p], gilbertList);
            }
            if (combinedMagneticField.magnitude == 0)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = null;
                continue;
            }
            Vector3 magneticFieldProjOntoPlaneNorm = Vector3.Dot(combinedMagneticField, getPlaneNormal()) * getPlaneNormal();
            Vector3 magneticFieldProjAlongPlane = combinedMagneticField - magneticFieldProjOntoPlaneNorm;
            arrowObjects[p].GetComponent<SpriteRenderer>().color = getColorFromFieldStrength(combinedMagneticField.magnitude);
            if (magneticFieldProjAlongPlane.magnitude * majoritySwitchThreshold >= magneticFieldProjOntoPlaneNorm.magnitude)
            {
                arrowObjects[p].GetComponent<SpriteRenderer>().sprite = pointingArrow;
                Quaternion toRotate = Quaternion.LookRotation(magneticFieldProjAlongPlane, this.transform.up);
                arrowObjects[p].transform.rotation = toRotate;
                arrowObjects[p].transform.Rotate(90, 0, 0, Space.Self);
            }
            else
            {
                if (Vector3.Angle(getPlaneNormal(), magneticFieldProjOntoPlaneNorm) == 0)
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = outArrow;
                }
                else
                {
                    arrowObjects[p].GetComponent<SpriteRenderer>().sprite = inArrow;
                }
            }
        }
    }

    private void setFieldPoints()
    {
        fieldPoints = new List<Vector3>();
        Vector3 localPosition = Vector3.zero;
        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                float xPos = (localPosition[0] - width / 2 + unitWidth / 2) + c * unitWidth;
                float zPos = (localPosition[1] - length / 2 + unitLength / 2) + r * unitLength;
                Vector3 localPoint = new Vector3(xPos, 0, zPos);
                fieldPoints.Add(transform.TransformPoint(localPoint));
            }
        }
    }

    private void setArrowObjects()
    {
        arrowObjects = new List<GameObject>();
        for(int i = 0; i < fieldPoints.Count; i++)
        {
            GameObject arrowObject = new GameObject("FieldArrow" + i,typeof(SpriteRenderer));
            arrowObject.transform.SetParent(this.transform);
            arrowObject.transform.position = fieldPoints[i];
            arrowObject.transform.LookAt(arrowObject.transform.position - this.transform.up);
            arrowObjects.Add(arrowObject);
        }
    }

    private Vector3 getPlaneNormal()
    {
        return transform.TransformDirection(Vector3.up);
    }

    private void debugDisplayPoints()
    {
        foreach (Vector3 point in fieldPoints)
        {
            Debug.DrawRay(point, transform.TransformDirection(Vector3.up), Color.red);
        }
    }
    private void debugDisplayBounds()
    {
        Vector3 topLeft = transform.TransformPoint(new Vector3(-width / 2, 0, length / 2));
        Vector3 topRight = transform.TransformPoint(new Vector3(width / 2, 0, length / 2));
        Vector3 botLeft = transform.TransformPoint(new Vector3(-width / 2, 0, -length / 2));
        Vector3 botRight = transform.TransformPoint(new Vector3(width / 2, 0, -length / 2));
        Debug.DrawRay(topLeft, width * transform.TransformDirection(Vector3.right), Color.red);
        Debug.DrawRay(topRight, length * transform.TransformDirection(Vector3.back), Color.red);
        Debug.DrawRay(botRight, width * transform.TransformDirection(Vector3.left), Color.red);
        Debug.DrawRay(botLeft, length * transform.TransformDirection(Vector3.forward), Color.red);
    }
    private void debugDisplayGrid()
    {
        for (int r = 0; r <= numRows; r++)
        {
            float zPos = (-length / 2) + r * unitLength;
            Vector3 rowPoint = transform.TransformPoint(new Vector3(-width / 2, 0, zPos));
            Debug.DrawRay(rowPoint, width * transform.TransformDirection(Vector3.right), Color.green);
        }
        for (int c = 0; c <= numCols; c++)
        {
            float xPos = (-width / 2) + c * unitWidth;
            Vector3 colPoint = transform.TransformPoint(new Vector3(xPos, 0, -length / 2));
            Debug.DrawRay(colPoint, length * transform.TransformDirection(Vector3.forward), Color.green);
        }
    }
}