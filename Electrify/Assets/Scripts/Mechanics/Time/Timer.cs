using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private Image circularFill;
    private LTDescr countDown;
    private void Awake()
    {
        Vector3 position = transform.position;
        position.z = 1;
        transform.position = position;
        GetComponent<RectTransform>().localScale = new Vector2(1,1);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void CountDown(float duration, Action action)
    {
        countDown = LeanTween.value(circularFill.gameObject, 0, 1, duration).setOnUpdate((float val) =>
        {
            circularFill.fillAmount = 1 - val;
        }).setOnComplete(() =>
        {
            action();
            Destroy(gameObject);
        });
    }

    public void CancelCountDown()
    {
        LeanTween.cancel(countDown.uniqueId);
        Destroy(gameObject);
    }
}
