using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private TextMeshProUGUI percentageText;
    public string Pretext = "";
    public void setFillPercentage(float fillPercentage)
    {
        imageFill.fillAmount = fillPercentage;
        percentageText.text = Pretext + (int)(fillPercentage*100) + "%";
    }

}
