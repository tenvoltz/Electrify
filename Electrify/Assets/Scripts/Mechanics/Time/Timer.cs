using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private Image circularFill;
    private void Awake()
    {
        GetComponent<RectTransform>().localScale = new Vector2(1,1);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void countDown(float duration, Action action)
    {
        LeanTween.value(circularFill.gameObject, 0, 1, duration).setOnUpdate((float val) =>
        {
            circularFill.fillAmount = 1 - val;
        }).setOnComplete(() =>
        {
            action();
            Destroy(gameObject);
        });
    }
}
